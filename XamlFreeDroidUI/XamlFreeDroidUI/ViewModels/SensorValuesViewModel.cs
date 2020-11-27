using XamlFreeDroidUI.Models;
using XamlFreeDroidUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;

namespace XamlFreeDroidUI.ViewModels
{
    public class SensorValuesViewModel : BaseViewModel, ISensorValuesViewModel
    {
        private SensorValueItem _selectedItem;
        //public bool IsLoading { get; set; }
        public ObservableCollection<SensorValueItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<SensorValueItem> ItemTapped { get; }

        public Command OpenMapCommand { get; }

        private ISensorValuesRepository _repository;

        public SensorValuesViewModel(ISensorValuesRepository repository)
        {
            Title = "Settings";
            Items = new ObservableCollection<SensorValueItem>();
            LoadItemsCommand = new Command(async () => await LoadItemsAsync());
            OpenMapCommand = new Command(async () => await OpenMapAsync());
            _repository = repository;
            //_repository = DependencyService.Get<ISettingsRepository>();
        }

        public async Task InitializeAsync()
        {
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            IsLoading = true;

            try
            {
                Items.Clear();
                var items = await _repository.GetSensorValuesAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OpenMapAsync()
        {

            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);
            var options = new MapLaunchOptions { Name = "Current Location" };

            //var location = new Location(40.6360546, -105.4586101);
            //var options = new MapLaunchOptions { Name = "Big Tree" };
            //await Map.OpenAsync(location, options);

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                // No map application available to open
                Console.WriteLine($"Can't open map: {ex.Message}");
            }
        }

        public void ExampleSetupWebCommand()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
        }

        public ICommand OpenWebCommand { get; set; }




    }
}



//navigation examples
//        private async void OnAddItem(object obj)
//        {
//            await Shell.Current.GoToAsync(nameof(NewItemPage));
//        }

//        async void OnItemSelected(Item item)
//        {
//            if (item == null)
//                return;

//            // This will push the ItemDetailPage onto the navigation stack
//            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
//        }
//private async void OnCancel()
//{
//    // This will pop the current page off the navigation stack
//    await Shell.Current.GoToAsync("..");
//}
