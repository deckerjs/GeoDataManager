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

namespace TrackDataDroid.ViewModels
{
    public class MapViewModel
    {
        private readonly CoordinateDataRepository _dataRepository;

        public MapViewModel(CoordinateDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public Command LoadItemsCommand { get; }

        public List<string> MapDataItems { get; }

        public async Task<MapView> GetMapViewAsync()
        {
            var map = GetCustomMap();

            var lineStringLayer = await CreateLineStringLayer(CreateLineStringStyle());            
            map.Layers.Add(lineStringLayer);

            map.Home = n => n.NavigateTo(lineStringLayer.Envelope.Centroid, 200);

            map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(map)
            {
                TextAlignment = Alignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            });

            return new MapView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = System.Drawing.Color.Black,
                Map = map
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

        private static IStyle CreateLineStringStyle()
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
            var map = new Mapsui.Map();
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

        //todo: add map choice options in configuration
        private Map GetOnlineOSMMap()
        {            
            var map = new Map
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

