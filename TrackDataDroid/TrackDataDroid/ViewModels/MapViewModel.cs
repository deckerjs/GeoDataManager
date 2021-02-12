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
using BruTile.Web;
using BruTile.Predefined;
using Microsoft.Extensions.Logging;

namespace TrackDataDroid.ViewModels
{
    public class MapViewModel: BaseViewModel
    {
        private readonly CoordinateDataRepository _dataRepository;
        private readonly ILogger<MapViewModel> _logger;
        private MapView _mapView;
        
        public MapViewModel(CoordinateDataRepository dataRepository, ILogger<MapViewModel> logger)
        {
            _dataRepository = dataRepository;
            _logger = logger;

            try
            {
                AvailableCoordinateData = new ObservableCollection<TrackSummaryViewModel>();
                AvailableTrackLayers = new ObservableCollection<LayerViewModel<CoordinateData>>();
                MapLayersFiltered = new ObservableCollection<ILayer>();
                MapFileLayers = new ObservableCollection<LayerViewModel<string>>();
                MapUrlLayers = new ObservableCollection<LayerViewModel<string>>();

                LoadAvailableTracksCommand = new Command(async () => await LoadAvailableTracks(), CanLoadAvailableTracks());
                LoadTrackCommand = new Command<TrackSummaryViewModel>(async (x) => await AddTrackLayer(x),CanLoadTrack());
                RemoveLoadedTrackCommand = new Command<LayerViewModel<CoordinateData>>((x) => RemoveTrackLayer(x), CanRemoveLoadedTrack());
                NavToLayerCenterCommand = new Command<ILayer>(x => CenterLayer(x), (x)=> x != null &&_mapView!=null);
                MoveLayerUpCommand = new Command<ILayer>(x => MoveLayerUp(x, _map.Layers), (x) => x != null && _mapView != null);
                MoveLayerDownCommand = new Command<ILayer>(x => MoveLayerDown(x, _map.Layers), (x) => x != null && _mapView != null);

                AddMBTileFileLayerCommand = new Command<LayerViewModel<string>>(x => AddMBTileFileLayer(x.LayerData, x.Name, _map));
                RemoveMBTileFileLayerCommand = new Command<LayerViewModel<string>>(x => RemoveMBTileFileLayer(x, _map));
                SelectFileCommand = new Command(async () => await SelectFileAsync());

                AddUrlLayerCommand = new Command<LayerViewModel<string>>(x => AddUrlLayer(x.LayerData, x.Name, _map));
                RemoveUrlLayerCommand = new Command<LayerViewModel<string>>(x => RemoveUrlLayer(x, _map));
            
                LayerViewModelEntry = new LayerViewModel<string>() { Name="example", LayerData= "https://a.tile.opentopomap.org/{z}/{x}/{y}.png"};

                UpdateDisplayInfo();
                DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;

                _map = GetCustomMap();
                _mapView = GetNewMapView();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
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
        public Command<ILayer> NavToLayerCenterCommand { get; }
        public Command<ILayer> MoveLayerUpCommand { get; }
        public Command<ILayer> MoveLayerDownCommand { get; }
        public Command<LayerViewModel<string>> AddMBTileFileLayerCommand { get; }
        public Command<LayerViewModel<string>> RemoveMBTileFileLayerCommand { get; }
        public Command<LayerViewModel<string>> AddUrlLayerCommand { get; }
        public Command<LayerViewModel<string>> RemoveUrlLayerCommand { get; }
        public Command SelectFileCommand { get; }

        
        private Mapsui.Map _map;
        public Mapsui.Map Map
        {
            get { return _map; }
            set { SetProperty(ref _map,value); }
        }

        public ObservableCollection<TrackSummaryViewModel> AvailableCoordinateData { get; private set; }
        public ObservableCollection<LayerViewModel<CoordinateData>> AvailableTrackLayers { get; private set; }
        
        public ObservableCollection<ILayer> MapLayersFiltered { get; private set; }
        public ObservableCollection<LayerViewModel<string>> MapFileLayers { get; private set; }
        public ObservableCollection<LayerViewModel<string>> MapUrlLayers { get; private set; }

        private LayerViewModel<string> _layerViewModelEntry;
        public LayerViewModel<string> LayerViewModelEntry
        {
            get { return _layerViewModelEntry; }
            set { SetProperty(ref _layerViewModelEntry, value); }
        }

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

        //todo: new utility could be made to accomplish this, might be used elswhere
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
            try
            {
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
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
                      
            _mapView.MyLocationLayer.UpdateMyLocation(new Position(currentLocation.X, currentLocation.Y),true );
            _mapView.MyLocationLayer.Name = "Current Location";
            
            return _mapView;
        }

        //todo: break binding from mapview, move somewhere
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

        //todo: move to mapsui utility as extension on repo inteface or something
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
                    CenterLayer(trackLayerVm.Layer);
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

        //todo: move to mapsui utility as extension on repo interface or something
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

        //todo: move to mapsui utility as extension on mapview
        private void CenterLayer(ILayer layer)
        {
            if (layer.Envelope != null)
            {
                _mapView.Navigator.NavigateTo(layer.Envelope.Centroid, 100);
            }
        }

        //todo: move to mapsui utility
        private static IStyle GetLineStringStyle()
        {
            return new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Mapsui.Styles.Color.FromString(StyleRepository.Primary_Color_Highlight), Width = 3 }
            };
        }        
        
        private Mapsui.Map GetCustomMap()
        {
            var map = new Mapsui.Map()
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };

            map.Layers.LayerAdded += MapLayerAdded;
            map.Layers.LayerRemoved += MapLayerRemoved;

            //todo: add configuration pick base map(s)
            var worldLayerFile = CreateFileFromResource("world.mbtiles");
            var baseLayerFile = CreateFileFromResource("co-full-base-dark-1.mbtiles");
            var trackLayerFile = CreateFileFromResource("co-full-tracks-only-dark-1.mbtiles");
            var roadLayerFile = CreateFileFromResource("co-full-roads-only-dark-1.mbtiles");

            AddMBTileFileLayer(worldLayerFile, "world", map);
            AddMBTileFileLayer(baseLayerFile, "base", map);
            AddMBTileFileLayer(trackLayerFile, "roads", map);
            AddMBTileFileLayer(roadLayerFile, "tracks", map);

            return map;
        }

        private void MapLayerRemoved(ILayer layer)
        {
            MapLayersFiltered.Remove(layer);
        }

        private void MapLayerAdded(ILayer layer)
        {
            MapLayersFiltered.Add(layer);
        }

        //todo: move to file utility
        private string CreateFileFromResource(string layerFileName)
        {
            WriteStreamToFile(layerFileName);
            return GetSpecialFolderPath(layerFileName);
        }

        private async Task SelectFileAsync()
        {
            try
            {                
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    var fileName = $"File Name: {result.FileName}";
                    if (result.FileName.EndsWith("mbtiles", StringComparison.OrdinalIgnoreCase))
                    {
                        AddMBTileFileLayer(result.FullPath, result.FileName, _map);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilePicker Exception: {ex.Message}");
            }
        }

        private void AddMBTileFileLayer(string path, string layerName, Mapsui.Map map)
        {
            if (File.Exists(path) && !string.IsNullOrEmpty(layerName) && map != null)
            {
                var layer = CreateMbTilesLayer(path, layerName);
                map.Layers.Add(layer);
                MapFileLayers.Add(new LayerViewModel<string>
                {
                    Name = layerName,
                    Description = layerName,
                    LayerData = path,
                    Layer = layer
                });
            }
            else
            {
                throw new Exception($"invalid file path: {path}");
            }
        }

        private void RemoveMBTileFileLayer(LayerViewModel<string> layerVm, Mapsui.Map map)
        {
            if (!string.IsNullOrEmpty(layerVm?.LayerData))
            {
                var layers = _map.Layers.Where(l => l.Name == layerVm?.Name).ToList();
                foreach (var layer in layers)
                {
                    _map.Layers.Remove(layer);
                }

                var fileLayers = MapFileLayers.Where(t => t.Name == layerVm.Name).ToList();
                foreach (var layer in fileLayers)
                {
                    MapFileLayers.Remove(layer);
                }
            }
        }

        private void AddUrlLayer(string url, string layerName, Mapsui.Map map)
        {
            if (!string.IsNullOrEmpty(layerName) && map != null)
            {
                var layer = CreateUrlTileLayer(url, layerName);
                map.Layers.Add(layer);
                MapUrlLayers.Add(new LayerViewModel<string>
                {
                    Name = layerName,
                    Description = layerName,
                    LayerData = url,
                    Layer = layer
                });
            }
            else
            {
                throw new Exception($"invalid url: {url}");
            }
        }

        private void RemoveUrlLayer(LayerViewModel<string> layerVm, Mapsui.Map map)
        {
            if (!string.IsNullOrEmpty(layerVm?.Name))
            {
                var layers = _map.Layers.Where(l => l.Name == layerVm?.Name).ToList();
                foreach (var layer in layers)
                {
                    _map.Layers.Remove(layer);
                }

                var urlLayers = MapFileLayers.Where(t => t.Name == layerVm.Name).ToList();
                foreach (var layer in urlLayers)
                {
                    MapFileLayers.Remove(layer);
                }
            }
        }


        private void MoveLayerUp(ILayer layer, LayerCollection layerCollection)
        {
            var layerIndex = GetLayerIndex(layer, layerCollection);
            if (layerIndex != -1 && layerIndex < MapLayersFiltered.Count())
            {
                layerCollection.Move(layerIndex + 1, layer);
                MapLayersFiltered.Move(layerIndex, layerIndex + 1);
                _mapView.Refresh();
            }
        }

        private void MoveLayerDown(ILayer layer, LayerCollection layerCollection)
        {
            var layerIndex = GetLayerIndex(layer, layerCollection);
            if (layerIndex != -1 && layerIndex > 0)
            {
                layerCollection.Move(layerIndex - 1, layer);
                MapLayersFiltered.Move(layerIndex, layerIndex - 1);
                _mapView.Refresh();
            }
        }

        //todo: move to mapsui utility
        private int GetLayerIndex(ILayer layer, LayerCollection layerCollection)
        {
            int index = 0;
            var comparer = EqualityComparer<ILayer>.Default;
            foreach (ILayer item in layerCollection)
            {
                if (comparer.Equals(item, layer)) return index;
                index++;
            }
            return -1;
        }


        //todo: move to file utility
        private static string GetSpecialFolderPath(string baseLayerfile)
        {
            //todo: look into sd card location for base maps
            var basepath = @"." + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(basepath, baseLayerfile);
        }

        //this is generic, any file included with project that needs to be read would use it
        //todo: move to file utility
        private static void WriteStreamToFile(string fileName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"TrackDataDroid.{fileName}");
            var fullPath = GetSpecialFolderPath(fileName);

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

        //todo: this is pretty generic, it could be added to some map utility 
        private static TileLayer CreateMbTilesLayer(string path, string name)
        {
            var sqliteconn = new SQLiteConnectionString(path, false);
            var mbTilesTileSource = new MbTilesTileSource(sqliteconn);
            var mbTilesLayer = new TileLayer(mbTilesTileSource) { Name = name };
            return mbTilesLayer;
        }

        //todo: move to mapsui utility
        private async Task<Mapsui.Geometries.Point> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            return new Mapsui.Geometries.Point(location.Latitude, location.Longitude);

        }


        //todo: move to mapsui utility
        private TileLayer CreateUrlTileLayer(string url, string name)
        {
            var tileSource = CreateTileSource(url, name);
            //todo: add option for osm tilesource
            //var tileSource = CreateOSMTileSource(url, name);

            return new TileLayer(tileSource) { Name = name};
        }

        //move to mapsui utility
        private HttpTileSource CreateTileSource(string url, string name)
        {
            return new HttpTileSource(
                    new GlobalSphericalMercator(), 
                    urlFormatter: url, //"https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    serverNodes: null, //new[] { "a", "b", "c" }, 
                    name: name, //"OpenStreetMap"
                    attribution: null, //new BruTile.Attribution("© OpenStreetMap contributors", "https://www.openstreetmap.org/copyright")
                    userAgent: null //"OpenStreetMap in Mapsui"
                    );
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

    }


}

