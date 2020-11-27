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
                    new Position(40.4410594,-105.4339465),
                    new Position(40.4410206,-105.3990132),
                    new Position(40.4230314,-105.3970818),
                    new Position(40.4166138,-105.4371445)
                }
            };

            Polyline polyline = new Polyline
            {
                StrokeColor = Color.Blue,
                StrokeWidth = 12,
                Geopath =
                {
                    new Position(40.6360546, -105.4586101),
                    new Position(40.9662941, -105.1849848),
                    new Position(39.9240271,-105.5508072)
                }
            };

            _map.MapElements.Add(polygon);
            _map.MapElements.Add(polyline);

            MapSpan mapspan = MapSpan.FromCenterAndRadius(new Position(40.6360546, -105.4586101), Distance.FromKilometers(0.444));
            _map.MoveToRegion(mapspan);

        }

    }


}
