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
        private readonly IConfiguration _configuration;

        public static IHost Host { get; private set; }

        public App()
        {            
            Device.SetFlags(new string[] { "Markup_Experimental" });            
            Resources = StyleRepository.DefaultStyle();
        }
        
        public App(IHost host, IConfiguration configuration) : this()
        {
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

        private ApiClientSettings GetApiClientSettings(IConfiguration config)
        {
            return config.GetSection("ApiClientSettings").Get<ApiClientSettings>();
        }
    }
}
