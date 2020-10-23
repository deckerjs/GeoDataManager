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
        private readonly SettingItemsPage _settingItemsPage;

        public AppShell(SettingItemsPage settingItemsPage)
        {
            //InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            _settingItemsPage = settingItemsPage;

            Resources = GetResources();
            Title = "App Shell Title";

            ShellSection shellSection1 = new ShellSection { Title = "Shell Section 1" };
            shellSection1.Items.Add(new ShellContent() { Content = _settingItemsPage });

            Items.Add(shellSection1);

            //var shellItem = new Xamarin.Forms.ShellItem();

            //CurrentItem = new TabBar();
            //{ 
            //    Items = new []{ new Xamarin.Forms.ShellItem { Title="stuff" }  }
            //}

            //CurrentItem.Items.Add(new ShellContent
            //{
            //    Title = "stuff",
            //    Icon = "tab_feed.png",
            //    ContentTemplate = new DataTemplate(typeof(SettingItemsPage))
            //});

            //ShellContent 
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

            //rd.Add("BaseStyle", baseStyle);

            Style<TabBar> tabBar = new Style<TabBar>().BasedOn(baseStyle);
            Style<FlyoutItem> flyoutItem = new Style<FlyoutItem>().BasedOn(baseStyle);


            return new ResourceDictionary() { baseStyle, tabBar, flyoutItem };
        }


    }
}
