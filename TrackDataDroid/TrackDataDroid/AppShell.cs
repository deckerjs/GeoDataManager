using System;
using System.Collections.Generic;
using TrackDataDroid.ViewModels;
using TrackDataDroid.Views;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace TrackDataDroid
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly OsmMapViewPage _osmMapViewPage;
        private readonly MapDataPage _mapDataPage;

        public AppShell(OsmMapViewPage osmMapViewPage, MapDataPage mapViewPage)
        {
            _osmMapViewPage = osmMapViewPage;
            _mapDataPage = mapViewPage;
            Resources = GetResources();
            Title = "Track Viewer";
            
            Items.Add(new TabBar 
            { 
                Title = "Shell Section MainPage",
                Items = 
                    {
                        new ShellContent() { Title="Data", Icon="tab_feed.png", Content = _mapDataPage},
                        new ShellContent() { Title="Map", Icon="tab_globe.svg", Content = _osmMapViewPage}
                    }
            });

        }

        // override styles
        public static ResourceDictionary GetResources()
        {
            Style<Element> baseStyle = new Style<Element>();
            Style<TabBar> tabBar = new Style<TabBar>().BasedOn(baseStyle);
            Style<FlyoutItem> flyoutItem = new Style<FlyoutItem>().BasedOn(baseStyle);

            return new ResourceDictionary() { baseStyle, tabBar, flyoutItem };
        }


    }
}
