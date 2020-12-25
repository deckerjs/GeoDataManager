using System;
using Xamarin.Forms;
using Mapsui.UI.Forms;
using Mapsui;
using Mapsui.Projection;
using Mapsui.Widgets;
using Mapsui.Styles;
using Mapsui.Layers;
using Mapsui.Geometries;
using System.Linq;
using Mapsui.Providers;
using System.IO;
using BruTile.MbTiles;
using SQLite;
using System.Reflection;
using TrackDataDroid.Repositories;
using System.Collections.Generic;
using CoordinateDataModels;
using System.Threading.Tasks;
using Mapsui.Utilities;
using Xamarin.Forms.Markup;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace TrackDataDroid.ViewModels
{
    public class MapViewModel: BaseViewModel
    {
        private readonly CoordinateDataRepository _dataRepository;
        
        private MapView _mapView;
        
        public MapViewModel(CoordinateDataRepository dataRepository)
        {
            _dataRepository = dataRepository;

            AvailableCoordinateData = new ObservableCollection<TrackSummaryViewModel>();
            AvailableTrackLayers = new ObservableCollection<LayerViewModel<CoordinateData>>();

            LoadAvailableTracksCommand = new Command(async () => await LoadAvailableTracks(), CanLoadAvailableTracks());
            LoadTrackCommand = new Command<TrackSummaryViewModel>(async (x) => await AddTrackLayer(x),CanLoadTrack());
            RemoveLoadedTrackCommand = new Command<LayerViewModel<CoordinateData>>((x) => RemoveTrackLayer(x), CanRemoveLoadedTrack());

            UpdateDisplayInfo();
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;

            _map = GetCustomMap();
            _mapView = GetNewMapView();
        }

        private Func<bool> CanLoadAvailableTracks()
        {
            return ()=> ReadyToLoadTracks;
        }

        private Func<TrackSummaryViewModel, bool> CanLoadTrack()
        {
            return (x) => ReadyToLoadTracks;
        }
        
        private Func<LayerViewModel<CoordinateData>, bool> CanRemoveLoadedTrack()
        {
            return (x) => AvailableTrackLayers.Where(t=>t.LayerData.ID == x?.LayerData?.ID).Any();
        }

        private bool _readyToLoadTracks;
        public bool ReadyToLoadTracks
        {
            get { return _readyToLoadTracks; }
            set 
            {
                SetProperty(ref _readyToLoadTracks , value);
                LoadAvailableTracksCommand.ChangeCanExecute();
                LoadTrackCommand.ChangeCanExecute();
            }
        }


        public Command LoadAvailableTracksCommand { get; }
        public Command<TrackSummaryViewModel> LoadTrackCommand { get; }
        public Command<LayerViewModel<CoordinateData>> RemoveLoadedTrackCommand { get; }

        
        private Mapsui.Map _map;
        public Mapsui.Map Map
        {
            get { return _map; }
            set { SetProperty(ref _map,value); }
        }

        public ObservableCollection<TrackSummaryViewModel> AvailableCoordinateData { get; private set; }
        public ObservableCollection<LayerViewModel<CoordinateData>> AvailableTrackLayers { get; private set; }
        
        private DisplayInfo _currentDisplayInfo;        
        public DisplayInfo CurrentDisplayInfo
        {
            get { return _currentDisplayInfo; }
            set { SetProperty(ref _currentDisplayInfo, value); }
        }

        private StackOrientation _currentStackOrientation;
        public StackOrientation CurrentStackOrientation
        {
            get { return _currentStackOrientation; }
            set { SetProperty(ref _currentStackOrientation, value); }            
        }

        private double _section1Width;
        public double Section1Width
        {
            get { return _section1Width; }
            set { SetProperty(ref _section1Width, value); }
        }

        private double _section1Height;
        public double Section1Height
        {
            get { return _section1Height; }
            set { SetProperty(ref _section1Height, value); }
        }

        private double _section2Width;
        public double Section2Width
        {
            get { return _section2Width; }
            set { SetProperty(ref _section2Width, value); }
        }

        private double _section2Height;
        public double Section2Height
        {
            get { return _section2Height; }
            set { SetProperty(ref _section2Height, value); }
        }

        private void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            UpdateDisplayInfo();
        }

        private void UpdateDisplayInfo()
        {
            CurrentDisplayInfo = DeviceDisplay.MainDisplayInfo;

            if (CurrentDisplayInfo.Width >= CurrentDisplayInfo.Height)
            {
                CurrentStackOrientation = StackOrientation.Horizontal;
                double threeQW = (CurrentDisplayInfo.Width / 4) * 3;
                double oneQW = CurrentDisplayInfo.Width - threeQW;

                Section1Height = CurrentDisplayInfo.Height;
                Section1Width = threeQW;

                Section2Height = CurrentDisplayInfo.Height;
                Section2Width = oneQW;
            }
            else
            {
                CurrentStackOrientation = StackOrientation.Vertical;
                double threeQH = (CurrentDisplayInfo.Height / 4) * 3;
                double oneQH = CurrentDisplayInfo.Height - threeQH;

                Section1Height = threeQH;
                Section1Width = CurrentDisplayInfo.Width;

                Section2Height = oneQH;
                Section2Width = CurrentDisplayInfo.Width;
            }
        }

        public async Task LoadAvailableTracks()
        {
            //todo: add track date to summary instead of track import date
            if (ReadyToLoadTracks)
            {
                var items = await GetAvailableTracks();
                AvailableCoordinateData.Clear();
                foreach (var item in items)
                {
                    AvailableCoordinateData.Add(item);
                }
            }
        }

        public async Task<MapView> GetMapViewAsync()
        {
            var currentLocation = await GetCurrentLocation();
            _map.Home = n => n.NavigateTo(SphericalMercator.FromLonLat(currentLocation.Y, currentLocation.X), 200);

            _map.Widgets.Add(new Mapsui.Widgets.Zoom.ZoomInOutWidget()
            {
                Enabled = true,
                StrokeColor = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Color),
                BackColor = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Color_Shaded),
                TextColor = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Color),
            });

            _map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(_map)
            {
                StrokeWidth = 1,
                TextColor = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Color_Highlight),
                Halo = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Black),
                Enabled = true,
                TextAlignment = Alignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            });
                      
            return _mapView;
        }

        private MapView GetNewMapView()
        {
            return new MapView()
            {
                IsNorthingButtonVisible = false,
                IsZoomButtonVisible = false,
                IsMyLocationButtonVisible = false,
                MyLocationEnabled = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Xamarin.Forms.Color.FromHex(StyleRepository.Primary_Black),
                Map = _map
            }
            .Bind(StackLayout.HeightRequestProperty, nameof(Section1Height))
            .Bind(StackLayout.WidthRequestProperty, nameof(Section1Width));
        }

        private async Task<List<TrackSummaryViewModel>> GetAvailableTracks()
        {
            if (!ReadyToLoadTracks) return null;
            var getResult = await _dataRepository.GetTracksSummaryAsync();
            List<TrackSummaryViewModel> trackSummary = getResult?.Select(x=> new TrackSummaryViewModel()
            { 
                CoordinateData = x               
            }).ToList();
            return trackSummary;
        }

        private async Task AddTrackLayer(TrackSummaryViewModel trackSummaryVm)
        {
            if (ReadyToLoadTracks && trackSummaryVm!=null && !string.IsNullOrEmpty(trackSummaryVm.CoordinateData?.ID))
            {
                var trackLayerVm = await GetTrackLayerVm(trackSummaryVm.CoordinateData.ID);
                if (trackLayerVm != null)
                {
                    _map.Layers.Add(trackLayerVm.Layer);
                    _mapView.Navigator.NavigateTo(trackLayerVm.Layer.Envelope.Centroid, 100);
                    AvailableTrackLayers.Add(trackLayerVm);
                }
            }
        }
        
        private void RemoveTrackLayer(LayerViewModel<CoordinateData> trackLayerVm)
        {
            if (!string.IsNullOrEmpty(trackLayerVm?.LayerData?.ID))
            {
                var layers = _map.Layers.Where(l => l.Name == trackLayerVm.LayerData.ID).ToList();
                foreach (var layer in layers)
                {
                    _map.Layers.Remove(layer);
                }

                var tracks = AvailableTrackLayers.Where(t => t.LayerData.ID == trackLayerVm.LayerData.ID).ToList();
                foreach (var track in tracks)
                {
                    AvailableTrackLayers.Remove(track);
                }
            }
        }

        private async Task<LayerViewModel<CoordinateData>> GetTrackLayerVm(string trackId)
        {
            var coordData = await _dataRepository.GetTrackAsync(trackId);

            if (coordData != null)
            {                
                //todo: loop through segments and add feature for each
                //add feature factory or something
                var trackPoints = coordData.Data.First().Coordinates.Select(x => SphericalMercator.FromLonLat(x.Longitude, x.Latitude));

                //todo: try to style starting and ending points differently
                var startingPoint = new Feature { Geometry = trackPoints.First() };
                var endingPoint = new Feature { Geometry = trackPoints.Last() };
                var trackLine = new Feature { Geometry = new LineString(trackPoints) };

                var features = new[] { startingPoint, endingPoint, trackLine };
                var memLayer = new MemoryLayer
                {                
                    DataSource = new MemoryProvider(features),
                    Name = coordData.ID,
                    Style = GetLineStringStyle()
                };

                LayerViewModel<CoordinateData> trackLayerVm = new LayerViewModel<CoordinateData>();
                trackLayerVm.Layer = memLayer;
                trackLayerVm.LayerData = coordData;

                return trackLayerVm;
            }
            return null;
        }

        private async Task<ILayer> CreateLineStringLayer(IStyle style = null)
        {

            var getResult = await _dataRepository.GetTracksSummaryAsync();
            List<CoordinateDataSummary> trackSummary = getResult?.ToList();

            if(trackSummary!=null && trackSummary.Count > 0)
            {
                var firstTrack = trackSummary.FirstOrDefault();
                var coordData = await _dataRepository.GetTrackAsync(firstTrack.ID);

                var trackPoints = coordData.Data.First().Coordinates.Select(x => SphericalMercator.FromLonLat(x.Longitude, x.Latitude));

                //todo: style starting and ending points differently
                var startingPoint = new Feature { Geometry = trackPoints.First() } ;
                var endingPoint = new Feature { Geometry = trackPoints.Last() };
                var trackLine = new Feature { Geometry = new LineString(trackPoints) };

                var features = new[] { startingPoint, endingPoint, trackLine };
                return new MemoryLayer
                {
                    //DataSource = new MemoryProvider(new Feature { Geometry = trackLine }),
                    DataSource = new MemoryProvider(features),
                    Name = "LineStringLayer",
                    Style = style
                };

            }

            return new Layer();
        }

        private static IStyle GetLineStringStyle()
        {
            return new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Color_Highlight), Width = 3 }
            };
        }        
        
        private static Mapsui.Map GetCustomMap()
        {
            var map = new Mapsui.Map()
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };

            //todo: add configuration pick base map(s)
            var worldLayerFile = "world.mbtiles";
            var baseLayerFile = "co-full-base-dark-1.mbtiles";
            var trackLayerFile = "co-full-tracks-only-dark-1.mbtiles";
            var roadLayerFile = "co-full-roads-only-dark-1.mbtiles";

            var worldLayerFullPath = GetFullPath(worldLayerFile);
            var baseLayerFullPath = GetFullPath(baseLayerFile);
            var trackLayerFullPath = GetFullPath(trackLayerFile);
            var roadLayerFullPath = GetFullPath(roadLayerFile);

            WriteStreamToFile(worldLayerFile);
            WriteStreamToFile(baseLayerFile);
            WriteStreamToFile(trackLayerFile);
            WriteStreamToFile(roadLayerFile);

            if (File.Exists(baseLayerFullPath) && File.Exists(trackLayerFullPath))
            {
                map.Layers.Add(CreateMbTilesLayer(worldLayerFullPath, "world"));
                map.Layers.Add(CreateMbTilesLayer(baseLayerFullPath, "base"));
                map.Layers.Add(CreateMbTilesLayer(roadLayerFullPath, "roads"));
                map.Layers.Add(CreateMbTilesLayer(trackLayerFullPath, "tracks"));
            }
            else
            {
                throw new Exception($"invalid file path: {baseLayerFullPath}");
            }

            return map;
        }

        private static string GetFullPath(string baseLayerfile)
        {
            //todo: look into sd card location for base maps
            var basepath = @"." + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(basepath, baseLayerfile);
        }

        private static void WriteStreamToFile(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TrackDataDroid.{fileName}");
            var fullPath = GetFullPath(fileName);

            using (BinaryReader br = new BinaryReader(stream))
            {
                using (BinaryWriter bw = new BinaryWriter(new FileStream(fullPath, FileMode.Create)))
                {
                    byte[] buffer = new byte[2048];
                    int len = 0;
                    while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, len);
                    }
                }
            }
        }

        private static TileLayer CreateMbTilesLayer(string path, string name)
        {
            var sqliteconn = new SQLiteConnectionString(path, false);
            var mbTilesTileSource = new MbTilesTileSource(sqliteconn);
            var mbTilesLayer = new TileLayer(mbTilesTileSource) { Name = name };
            return mbTilesLayer;
        }


        private async Task<Mapsui.Geometries.Point> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            return new Mapsui.Geometries.Point(location.Latitude, location.Longitude);

        }


        //todo: add map choice options in configuration
        private Mapsui.Map GetOnlineOSMMap()
        {            
            var map = new Mapsui.Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };
            map.Layers.Add(OpenStreetMap.CreateTileLayer());
            return map;
        }

        //demo testing only, remove when done
        public ILayer GetDemoLineStringLayer(IStyle style = null)
        {
            string WKTGr5 = "LINESTRING(40.939812306314707 -104.4140350073576, 40.770964482799172 -105.78512543812394, 40.884347157552838 -105.83570159040391, 40.804107449948788 -105.66309272311628)";
        var lineString = (LineString)Geometry.GeomFromText(WKTGr5);
            lineString = new LineString(lineString.Vertices.Select(v => SphericalMercator.FromLonLat(v.Y, v.X)));

            return new MemoryLayer
            {
                DataSource = new MemoryProvider(new Feature { Geometry = lineString }),
                Name = "LineStringLayer",
                Style = style
            };
        }

    }


}

