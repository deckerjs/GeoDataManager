using System;
using System.Collections.Generic;
using XamlFreeDroidUI.ViewModels;
using XamlFreeDroidUI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace XamlFreeDroidUI
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly MainPage _mainPage;
        private readonly SensorItemsPage _settingItemsPage;
        private readonly MapViewPage _mapViewPage;
        private readonly OsmMapViewPage _osmMapViewPage;

        public AppShell(MainPage mainPage, SensorItemsPage settingItemsPage, MapViewPage mapViewPage, OsmMapViewPage osmMapViewPage)
        {
            _mainPage = mainPage;
            _settingItemsPage = settingItemsPage;
            _mapViewPage = mapViewPage;
            _osmMapViewPage = osmMapViewPage;
            Resources = GetResources();
            Title = "App Shell Title";
            
            // bottom tab bar
            Items.Add(new TabBar 
            { 
                Title = "Shell Section MainPage",
                Items = 
                    {
                        new ShellContent() { Title="Main", Icon="tab_feed.png", Content = _mainPage },            
                        new ShellContent() { Title="Sensor Items", Icon="tab_feed.png", Content = _settingItemsPage },
                        new ShellContent() { Title="Default Map", Icon="tab_feed.png", Content = _mapViewPage },
                        new ShellContent() { Title="Osm Map", Icon="tab_feed.png", Content = _osmMapViewPage }
                    }
            });

            // or the corner menu
            //Items.Add(new ShellSection
            //{
            //    Title = "Shell Section MainPage",
            //    Items =
            //        {
            //            new ShellContent() { Title="MainPage", Icon="tab_feed.png", Content = _mainPage }
            //        }
            //});
            //Items.Add(new ShellSection
            //{
            //    Title = "Shell Section SensorItems",
            //    Items =
            //        {
            //            new ShellContent() { Title="SettingItemsPage", Icon="tab_feed.png", Content = _settingItemsPage }
            //        }
            //});

            //routes to additional pages
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        // override styles
        public static ResourceDictionary GetResources()
        {
            Style<Element> baseStyle = new Style<Element>(
                //(Shell.BackgroundColorProperty, Color.Black),
                );
            Style<TabBar> tabBar = new Style<TabBar>().BasedOn(baseStyle);
            Style<FlyoutItem> flyoutItem = new Style<FlyoutItem>().BasedOn(baseStyle);

            return new ResourceDictionary() { baseStyle, tabBar, flyoutItem };
        }


    }
}
