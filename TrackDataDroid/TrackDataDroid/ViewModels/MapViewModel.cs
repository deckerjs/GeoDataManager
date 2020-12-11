﻿using System;
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
        private bool _readyToLoadTracks = false;
        private Mapsui.Map _map;
        private MapView _mapView;

        public MapViewModel(CoordinateDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            AvailableCoordinateData = new ObservableCollection<TrackSummaryViewModel>();
            LoadAvailableTracksCommand = new Command(async () => await LoadAvailableTracks(), CanLoadAvailableTracks());
            LoadTrackCommand = new Command<string>(async (x) => await AddTrackLayerToMap(x),CanLoadTrack());

            UpdateDisplayInfo();
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }

        private Func<bool> CanLoadAvailableTracks()
        {
            return ()=> _readyToLoadTracks;
        }

        private Func<string, bool> CanLoadTrack()
        {
            return (x) => _readyToLoadTracks;
        }

        public Command LoadAvailableTracksCommand { get; }
        public Command<string> LoadTrackCommand { get; }
        public ObservableCollection<TrackSummaryViewModel> AvailableCoordinateData { get; private set; }
        
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
            set { SetProperty(ref _section1Width ,value); }
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
            set { SetProperty(ref _section2Width ,value); }
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
                double oneQW = CurrentDisplayInfo.Width-threeQW;

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
            //todo: create viewmodel for track summary data
            // include properties for
            // bool: ShowOnMap/selected
            // (once track is loaded) - track 


            if (_readyToLoadTracks)
            {
                var items = await GetAvailableTracks();
                foreach (var item in items)
                {
                    AvailableCoordinateData.Add(item);
                }

                if (items!=null && items.Any())
                {
                    await AddTrackLayerToMap(items.First().CoordinateData.ID);
                }
            }
        }

        public async Task<MapView> GetMapViewAsync()
        {
            _map = GetCustomMap();

            //load data for track summary list, use for binding to list control
            //command can pick which track(s) to render

            //var lineStringLayer = await CreateLineStringLayer(GetLineStringStyle());            
            //map.Layers.Add(lineStringLayer);
            //map.Home = n => n.NavigateTo(lineStringLayer.Envelope.Centroid, 200);

            var currentLocation = await GetCurrentLocation();
            _map.Home = n => n.NavigateTo(SphericalMercator.FromLonLat(currentLocation.Y, currentLocation.X), 200);

            _map.Widgets.Add(new Mapsui.Widgets.Zoom.ZoomInOutWidget()
            {
                Enabled = true,
                BackColor = Mapsui.Styles.Color.Black,
                TextColor = Mapsui.Styles.Color.Orange
            });

            _map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(_map)
            {
                StrokeWidth = 1,
                TextColor = Mapsui.Styles.Color.Orange,
                Halo = Mapsui.Styles.Color.Black,
                Enabled = true,
                TextAlignment = Alignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            });

            _readyToLoadTracks = true;

            _mapView = new MapView()
            {
                IsNorthingButtonVisible= true,
                IsZoomButtonVisible=false,
                IsMyLocationButtonVisible=true,
                MyLocationEnabled =true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = System.Drawing.Color.Black,
                Map = _map
            }.Bind(StackLayout.HeightRequestProperty, nameof(Section1Height))
            .Bind(StackLayout.WidthRequestProperty, nameof(Section1Width));

            return _mapView;
        }

        private async Task<List<TrackSummaryViewModel>> GetAvailableTracks()
        {
            if (!_readyToLoadTracks) return null;

            var getResult = await _dataRepository.GetTracksSummaryAsync();

            List<TrackSummaryViewModel> trackSummary = getResult?.Select(x=> new TrackSummaryViewModel()
            { 
                CoordinateData = x,
                ShowOnMap = false                
            }).ToList();
            return trackSummary;
        }

        private async Task AddTrackLayerToMap(string trackId)
        {
            if (_readyToLoadTracks && ! string.IsNullOrEmpty(trackId))
            {
                var lineStringLayer = await GetTrackLayer(trackId);
                _map.Layers.Add(lineStringLayer);                
                _mapView.Navigator.NavigateTo(lineStringLayer.Envelope.Centroid, 100);
            }
        }

        private async Task<ILayer> GetTrackLayer(string trackId)
        {
            var coordData = await _dataRepository.GetTrackAsync(trackId);
            var trackPoints = coordData.Data.First().Coordinates.Select(x => SphericalMercator.FromLonLat(x.Longitude, x.Latitude));

            //todo: try to style starting and ending points differently
            var startingPoint = new Feature { Geometry = trackPoints.First() };
            var endingPoint = new Feature { Geometry = trackPoints.Last() };
            var trackLine = new Feature { Geometry = new LineString(trackPoints) };

            var features = new[] { startingPoint, endingPoint, trackLine };
            return new MemoryLayer
            {                
                DataSource = new MemoryProvider(features),
                Name = "LineStringLayer",
                Style = GetLineStringStyle()
            };

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
            //todo: add configuration to pick line string style. (color,width)
            return new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Mapsui.Styles.Color.Yellow, Width = 4 }
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

