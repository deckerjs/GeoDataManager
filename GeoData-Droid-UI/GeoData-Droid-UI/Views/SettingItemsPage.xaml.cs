using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sensortest.Services;
using sensortest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sensortest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingItemsPage : ContentPage
    {
        private ISensorValuesViewModel _viewModel;
        public SettingItemsPage()
        {
            InitializeComponent();
            //ISettingsRepository repository = DependencyService.Get<ISettingsRepository>();
            //BindingContext = _viewModel = new SettingsViewModel(repository);

            //_viewModel = DependencyService.Get<ISettingsViewModel>();

            //var stuff = DependencyService.Resolve<Stuff>();
            //_viewModel = DependencyService.Resolve<ISettingsViewModel>();

            var stuff = App.Host.Services.GetRequiredService<Stuff>();
            _viewModel = App.Host.Services.GetRequiredService<ISensorValuesViewModel>();

            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task viewModelInitTask = _viewModel.InitializeAsync();
        }
    }
}