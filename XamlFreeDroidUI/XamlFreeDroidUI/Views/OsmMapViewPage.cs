using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Mapsui.UI.Forms;
using Mapsui;
using Mapsui.Projection;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Styles;
using Mapsui.Layers;
using Mapsui.Geometries;
using System.Linq;
using Mapsui.Providers;

namespace XamlFreeDroidUI.Views
{
    public class OsmMapViewPage : ContentPage
    {

        public OsmMapViewPage()
        {
            Content = GetContent();

        }

        private View GetContent()
        {
            var map = new Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };

            map.Layers.Add(OpenStreetMap.CreateTileLayer());

            var lineStringLayer = CreateLineStringLayer(CreateLineStringStyle());
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
                        BackgroundColor = System.Drawing.Color.Gray,
                        Map = map                        
                    }
                }
            };

            return stackLayout;

        }


        public static ILayer CreateLineStringLayer(IStyle style = null)
        {
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
                Line = { Color = Mapsui.Styles.Color.FromString("YellowGreen"), Width = 4 }
            };
        }

        private const string WKTGr5 = "LINESTRING(51.494894027709961 4.295525550842285, 51.494894027709961 4.295740127563477, 51.49467945098877 4.295697212219238, 51.494636535644531 4.295718669891357, 51.494615077972412 4.295804500579834, 51.494615077972412 4.29591178894043, 51.494593620300293 4.29614782333374, 51.494572162628174 4.296340942382813, 51.494572162628174 4.296555519104004, 51.494550704956055 4.296748638153076, 51.494486331939697 4.296748638153076, 51.494486331939697 4.297542572021484, 51.49442195892334 4.298250675201416, 51.494293212890625 4.299302101135254, 51.493842601776123 4.299130439758301, 51.49367094039917 4.299066066741943, 51.49341344833374 4.298958778381348, 51.492898464202881 4.298744201660156, 51.492919921875 4.29990291595459, 51.492941379547119 4.300224781036377, 51.493070125579834 4.300782680511475, 51.493113040924072 4.300975799560547, 51.493263244628906 4.301769733428955, 51.493349075317383 4.302220344543457, 51.493370532989502 4.302327632904053, 51.493391990661621 4.302434921264648, 51.49341344833374 4.302542209625244, 51.493434906005859 4.30264949798584, 51.493456363677979 4.302799701690674, 51.493477821350098 4.302949905395508, 51.493520736694336 4.303250312805176, 51.493563652038574 4.303486347198486, 51.493606567382813 4.303679466247559, 51.493649482727051 4.303872585296631, 51.493713855743408 4.304172992706299, 51.493778228759766 4.304580688476563, 51.493864059448242 4.305438995361328, 51.493885517120361 4.3056321144104, 51.493949890136719 4.306426048278809, 51.493992805480957 4.306769371032715, 51.494078636169434 4.307692050933838, 51.494078636169434 4.307881146669388, 51.494078636169434 4.308078289031982, 51.493713855743408 4.308056831359863, 51.493628025054932 4.308078289031982, 51.49341344833374 4.308185577392578, 51.493370532989502 4.308185577392578, 51.493327617645264 4.30814266204834, 51.493220329284668 4.30788516998291, 51.493155956268311 4.307799339294434, 51.492340564727783 4.307734966278076, 51.491696834564209 4.307241439819336, 51.491267681121826 4.306812286376953)";

    }


}
