using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace XamlFreeDroidUI.Views
{


    public class MapViewPage : ContentPage
    {
    private Map _map;
        public MapViewPage()
        {

            _map = new Map();
            AddMapStuff();

            Content = _map;
        }


        private void AddMapStuff()
        {

            Polygon polygon = new Polygon
            {
                StrokeWidth = 8,
                StrokeColor = Color.FromHex("#1BA1E2"),
                FillColor = Color.FromHex("#881BA1E2"),
                Geopath =
                {
                    new Position(47.6368678, -122.137305),
                    new Position(47.6368894, -122.134655),
                    new Position(47.6359424, -122.134655),
                    new Position(47.6359496, -122.1325521),
                    new Position(47.6424124, -122.1325199),
                    new Position(47.642463,  -122.1338932),
                    new Position(47.6406414, -122.1344833),
                    new Position(47.6384943, -122.1361248),
                    new Position(47.6372943, -122.1376912)
                }
            };

            Polyline polyline = new Polyline
            {
                StrokeColor = Color.Blue,
                StrokeWidth = 12,
                Geopath =
                {
                    new Position(47.6381401, -122.1317367),
                    new Position(47.6381473, -122.1350841),
                    new Position(47.6382847, -122.1353094),
                    new Position(47.6384582, -122.1354703),
                    new Position(47.6401136, -122.1360819),
                    new Position(47.6403883, -122.1364681),
                    new Position(47.6407426, -122.1377019),
                    new Position(47.6412558, -122.1404056),
                    new Position(47.6414148, -122.1418647),
                    new Position(47.6414654, -122.1432702)
                }
            };

            _map.MapElements.Add(polygon);
            _map.MapElements.Add(polyline);

            MapSpan mapspan = MapSpan.FromCenterAndRadius(new Position(47.6414654, -122.1432702), Distance.FromKilometers(0.444));
            _map.MoveToRegion(mapspan);

        }

    }


}
