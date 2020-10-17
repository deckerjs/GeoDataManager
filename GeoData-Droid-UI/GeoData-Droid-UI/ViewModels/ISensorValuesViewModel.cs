using sensortest.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace sensortest.ViewModels
{
    public interface ISensorValuesViewModel
    {
        Command AddItemCommand { get; }
        ObservableCollection<SensorValueItem> Items { get; }
        Command<SensorValueItem> ItemTapped { get; }
        Command LoadItemsCommand { get; }

        Task InitializeAsync();
    }
}