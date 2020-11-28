using TrackDataDroid.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrackDataDroid.ViewModels
{
    public interface ISensorValuesViewModel: IBaseViewModel
    {
        Command AddItemCommand { get; }
        ObservableCollection<SensorValueItem> Items { get; }
        Command<SensorValueItem> ItemTapped { get; }
        Command LoadItemsCommand { get; }
        Command OpenMapCommand { get; }

        Task InitializeAsync();
    }
}