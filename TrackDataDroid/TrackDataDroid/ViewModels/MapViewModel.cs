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

namespace TrackDataDroid.ViewModels
{
    public class MapViewModel
    {
        private readonly CoordinateDataRepository _dataRepository;

        public MapViewModel(CoordinateDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<View> GetContentAsync()
        {
            // for online osm map
            //var map = new Map
            //{
            //    CRS = "EPSG:3857",
            //    Transformation = new MinimalTransformation()
            //};
            //map.Layers.Add(OpenStreetMap.CreateTileLayer());

            // open custom raster mbtiles map file
            var map = GetCustomMap();

            var lineStringLayer = await CreateLineStringLayer(CreateLineStringStyle());
            map.Layers.Add(lineStringLayer);
            map.Home = n => n.NavigateTo(lineStringLayer.Envelope.Centroid, 200);

            map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(map)
            {
                TextAlignment = Alignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            });

            var stackLayout = new StackLayout
            {
                Children =
                {
                new MapView()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions=LayoutOptions.Fill,
                        BackgroundColor = System.Drawing.Color.Black,
                        Map = map
                    }
                }
            };
            return stackLayout;
        }

        public async Task<ILayer> CreateLineStringLayer(IStyle style = null)
        {

            List<CoordinateDataSummary> trackSummary = (await _dataRepository.GetTracksSummaryAsync()).ToList();


            var lineString = (LineString)Geometry.GeomFromText(WKTGr5);
            lineString = new LineString(lineString.Vertices.Select(v => SphericalMercator.FromLonLat(v.Y, v.X)));

            return new MemoryLayer
            {
                DataSource = new MemoryProvider(new Feature { Geometry = lineString }),
                Name = "LineStringLayer",
                Style = style
            };
        }

        public static IStyle CreateLineStringStyle()
        {
            return new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Mapsui.Styles.Color.Green, Width = 4 }
            };
        }

        private const string WKTGr5 = "LINESTRING(40.939812306314707 -104.4140350073576, 40.770964482799172 -105.78512543812394, 40.884347157552838 -105.83570159040391, 40.804107449948788 -105.66309272311628)";
        private readonly MapViewModel _viewModel;

        private static Mapsui.Map GetCustomMap()
        {
            var map = new Mapsui.Map();

            //var file = "world.mbtiles";            
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
            //todo: decide which location is better for an included basemap
            //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
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

        public static TileLayer CreateMbTilesLayer(string path, string name)
        {
            var sqliteconn = new SQLiteConnectionString(path, false);
            var mbTilesTileSource = new MbTilesTileSource(sqliteconn);
            var mbTilesLayer = new TileLayer(mbTilesTileSource) { Name = name };
            return mbTilesLayer;
        }
    }


}

