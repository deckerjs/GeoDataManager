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

        public AppShell(MainPage mainPage, SensorItemsPage settingItemsPage)
        {
            _mainPage = mainPage;
            _settingItemsPage = settingItemsPage;

            Resources = GetResources();
            Title = "App Shell Title";
            
            // bottom tab bar
            Items.Add(new TabBar 
            { 
                Title = "Shell Section MainPage",
                Items = 
                    {
                        new ShellContent() { Title="Main Page", Icon="tab_feed.png", Content = _mainPage },            
                        new ShellContent() { Title="Sensor Items Page", Icon="tab_feed.png", Content = _settingItemsPage }
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
