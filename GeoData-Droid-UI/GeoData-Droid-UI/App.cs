using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using sensortest.Services;
using sensortest.Views;
using sensortest.Models;
using sensortest.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xamarin.Forms.Markup;

namespace sensortest
{
    public partial class App : Application
    {

        public static IHost Host { get; private set; }

        public App()
        {
            //InitializeComponent();
            Device.SetFlags(new string[] { "Markup_Experimental" });
            DependencyService.Register<IDataStore<Item>, MockDataStore>();




            Resources = DefaultStyle();

            //DependencyService.Register<ISettingsViewModel, SettingsViewModel>();
            //DependencyService.Register<ISettingsRepository, SettingsRepository>();
            //DependencyService.Register<Stuff>();
            //MainPage = new AppShell();
        }
        
        public App(IHost host) : this()
        {
            Host = host;
        }

        public static IHostBuilder BuildHost()
        {
            return XamarinHost.CreateDefaultBuilder<App>()
             .ConfigureServices((context, services) =>
             {
                 // may actually want to pull from some views from di host as needed 
                 // instead of all at start

                 services.AddScoped<AppShell>();
                 services.AddScoped<MainPage>();
                 services.AddScoped<SettingItemsPage>();

                 services.AddScoped<MainPageViewModel>();
                 services.AddScoped<ISensorValuesViewModel, SensorValuesViewModel>();
                                  
                 services.AddScoped<ISensorValuesRepository, SensorValuesRepository>();
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
            Style<ContentPage> contentPage = new Style<ContentPage>(
                (ContentPage.BackgroundColorProperty, Color.Black)
                );

            Style<Button> buttons = new Style<Button>(
                (Button.BackgroundColorProperty, Color.Black),
                (Button.TextColorProperty, Color.BlanchedAlmond),
                (Button.BorderColorProperty, Color.Green),
                (Button.BorderWidthProperty, 2)
                );




            return new ResourceDictionary() { contentPage, buttons };
        }

    }
}
