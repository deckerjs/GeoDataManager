using System;
using System.Collections.Generic;
using TrackDataDroid.Configuration;
using TrackDataDroid.Services;
using TrackDataDroid.ViewModels;
using TrackDataDroid.Views;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace TrackDataDroid
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly ConfigurationSettingsPage _settingsPage;
        private readonly OsmMapViewPage _osmMapViewPage;
        private readonly MapDataPage _mapDataPage;
        private readonly MapFileLayerPage _mapFileLayerPage;
        private readonly GpsPage _gpsPage;

        public AppShell(
            ConfigurationSettingsPage settingsPage,
            OsmMapViewPage osmMapViewPage, 
            MapDataPage mapViewPage,
            MapFileLayerPage mapFileLayerPage,
            GpsPage gpsPage)
        {
            _settingsPage = settingsPage;
            _osmMapViewPage = osmMapViewPage;
            _mapDataPage = mapViewPage;
            _mapFileLayerPage = mapFileLayerPage;
            _gpsPage = gpsPage;
            Resources = GetResources();
            Title = "Track Viewer";
                        
            Items.Add(new TabBar 
            { 
                Title = "Shell Section MainPage",
                Items = 
                    {
                        new ShellContent()
                            {
                                Title="Config",
                                Icon=ImageUtility.GetFontImageSource(IconNameConstants.Cog, ImageUtility.DefaultImageSize.Large),
                                Content = _settingsPage
                            },
                        new ShellContent() 
                            { 
                                Title="Data", 
                                Icon=ImageUtility.GetFontImageSource(IconNameConstants.LayerGroup, ImageUtility.DefaultImageSize.Large),
                                Content = _mapDataPage
                            },
                        new ShellContent() 
                            { 
                                Title="File Layers", 
                                Icon=ImageUtility.GetFontImageSource(IconNameConstants.LayerGroup, ImageUtility.DefaultImageSize.Large),
                                Content = _mapFileLayerPage
                            },
                        new ShellContent() 
                            { 
                                Title="Map",
                                Icon=ImageUtility.GetFontImageSource(IconNameConstants.GlobeAmericas, ImageUtility.DefaultImageSize.Large),
                                Content = _osmMapViewPage
                            },
                        new ShellContent()
                            {
                                Title="GPS",
                                Icon=ImageUtility.GetFontImageSource(IconNameConstants.Satellite, ImageUtility.DefaultImageSize.Large),
                                Content = _gpsPage
                            }
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
