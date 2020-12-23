using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TrackDataDroid.Services;
using TrackDataDroid.Views;
using TrackDataDroid.Models;
using TrackDataDroid.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xamarin.Forms.Markup;
using GeoStoreApi.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TrackDataDroid.Repositories;

namespace TrackDataDroid
{
    public partial class App : Application
    {
        private const string Primary_Text_Color = "#209DF7";
        private const string Primary_Page_Background_Color = "#252526";
        private const string Primary_Alt_Background_Color = "#333337";
        private readonly IConfiguration _configuration;

        public static IHost Host { get; private set; }

        public App()
        {            
            Device.SetFlags(new string[] { "Markup_Experimental" });            
            Resources = DefaultStyle();
        }
        
        public App(IHost host, IConfiguration configuration) : this()
        {
            //IConfiguration configuration
            Host = host;
            _configuration = configuration;
        }

        public static IHostBuilder BuildHost()
        {
            return XamarinHost.CreateDefaultBuilder<App>()
             .ConfigureServices((context, services) =>
             {
                 // todo: 
                 // may actually want to pull views from di host when shown
                 // instead of all at start 
                 // using something like App.Host.Services.GetRequiredService<something>

                 // todo: also may want to add each view,viewmodel,repo in its own method/extension
                 services.AddSingleton<AppShell>();

                 services.AddSingleton<ConfigurationSettingsPage>();
                 services.AddSingleton<OsmMapViewPage>();
                 services.AddSingleton<MapDataPage>();
                 services.AddSingleton<GpsPage>();



                 //services.AddScoped<MainPageViewModel>();
                 //services.AddScoped<ISensorValuesViewModel, SensorValuesViewModel>();
                 //services.AddScoped<ISensorValuesRepository, SensorValuesRepository>();

                 //services.Configure<AppOptions>(Configuration.GetSection("AppOptions"));
                 //services.AddScoped<AppOptions>(x => x.GetService<IOptions<AppOptions>>().Value);
                 
                 services.Configure<ApiClientSettings>(context.Configuration.GetSection("ApiClientSettings"));
                 services.AddSingleton<ApiClientSettings>(x =>
                 {
                     return Microsoft.Extensions.DependencyInjection
                     .ServiceProviderServiceExtensions.GetService<IOptions<ApiClientSettings>>(x).Value;
                     //return x.GetService<IOptions<ApiClientSettings>>().Value;
                 });

                 services.AddSingleton<MapViewModel>();
                 services.AddSingleton<CoordinateDataSourceFactory>();

                 services.AddScoped<CoordinateDataRepository>();
                 services.AddScoped<CoordinateDataOnlineSource>();
                 services.AddScoped<CoordinateDataOfflineSource>();

                 services.AddScoped<CoordinateDataApiClient>();



             });
        }

        protected override void OnStart()
        {
            Task.Run(async () => await Host.StartAsync());
            MainPage = Host.Services.GetRequiredService<AppShell>();
        }

        protected override void OnSleep()
        {
            Task.Run(async () => await Host.SleepAsync());
        }

        protected override void OnResume()
        {
            Task.Run(async () => await Host.ResumeAsync());
        }


        public static ResourceDictionary DefaultStyle()
        {

            Style<Element> baseStyle = new Style<Element>(
             (Shell.BackgroundColorProperty, Color.Black),
             (Shell.ForegroundColorProperty, Primary_Text_Color),
             (Shell.TitleColorProperty, "#5695D8"),
             (Shell.DisabledColorProperty, "#404040"),
             (Shell.UnselectedColorProperty, "#2763DB"),
             (Shell.TabBarBackgroundColorProperty, Color.Black),
             (Shell.TabBarForegroundColorProperty, Primary_Text_Color)
             );

            Style<Shell> shell = new Style<Shell>().BasedOn(baseStyle);
            Style<TabBar> tabBar = new Style<TabBar>().BasedOn(baseStyle);
            Style<FlyoutItem> flyoutItem = new Style<FlyoutItem>().BasedOn(baseStyle);

            Style<ContentPage> contentPage = new Style<ContentPage>(
                (ContentPage.BackgroundColorProperty, Primary_Page_Background_Color)
                ).ApplyToDerivedTypes(true);
            

            Style<Label> label = new Style<Label>(
                (Label.BackgroundColorProperty, Primary_Page_Background_Color),
                (Label.TextColorProperty, Primary_Text_Color));

            Style<CollectionView> collectionView = new Style<CollectionView>(
                (CollectionView.BackgroundColorProperty, Primary_Page_Background_Color));

            Style<Button> button = new Style<Button>(
                (Button.BackgroundColorProperty, Primary_Alt_Background_Color),
                (Button.TextColorProperty, Primary_Text_Color),
                (Button.BorderColorProperty, "#404040"),
                (Button.BorderWidthProperty, 2)
                );
            
            return new ResourceDictionary() 
                { 
                    baseStyle,
                    shell,
                    tabBar, 
                    flyoutItem,
                    contentPage, 
                    collectionView, 
                    label, 
                    button 
                };
        }


        private ApiClientSettings GetApiClientSettings(IConfiguration config)
        {
            return config.GetSection("ApiClientSettings").Get<ApiClientSettings>();
        }
    }
}
