using System.ComponentModel;
using Xamarin.Forms;
using sensortest.ViewModels;

namespace sensortest.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}