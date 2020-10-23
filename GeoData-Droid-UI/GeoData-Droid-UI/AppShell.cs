using System;
using System.Collections.Generic;
using sensortest.ViewModels;
using sensortest.Views;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace sensortest
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly MainPage _mainPage;
        private readonly SettingItemsPage _settingItemsPage;

        public AppShell(MainPage mainPage, SettingItemsPage settingItemsPage)
        {
            _mainPage = mainPage;
            _settingItemsPage = settingItemsPage;

            Resources = GetResources();
            Title = "App Shell Title";

            Items.Add(new ShellSection 
            { 
                Title = "Shell Section MainPage",
                Items = 
                    {
                        new ShellContent() { Title="MainPage", Icon="tab_feed.png", Content = _mainPage }            
                    }
            });

            Items.Add(new ShellSection 
            { 
                Title = "Shell Section SensorItems",
                Items = 
                    {
                        new ShellContent() { Title="SettingItemsPage", Icon="tab_feed.png", Content = _settingItemsPage }
                    }
            });

            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

        }

        public static ResourceDictionary GetResources()
        {
            var rd = new ResourceDictionary();

            Style<Element> baseStyle = new Style<Element>(
                (Shell.BackgroundColorProperty, Color.Black),
                (Shell.ForegroundColorProperty, Color.BlanchedAlmond),
                (Shell.TitleColorProperty, "#1D1"),
                (Shell.DisabledColorProperty, "#151"),
                (Shell.UnselectedColorProperty, "#171"),
                (Shell.TabBarBackgroundColorProperty, Color.DarkBlue),
                (Shell.TabBarForegroundColorProperty, Color.BlanchedAlmond)
                );

            Style<TabBar> tabBar = new Style<TabBar>().BasedOn(baseStyle);
            Style<FlyoutItem> flyoutItem = new Style<FlyoutItem>().BasedOn(baseStyle);

            return new ResourceDictionary() { baseStyle, tabBar, flyoutItem };
        }


    }
}
