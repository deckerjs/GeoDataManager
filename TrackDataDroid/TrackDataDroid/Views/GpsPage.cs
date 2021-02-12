using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;
using CoordinateDataModels;
using Xamarin.Essentials;
using System;
using TrackDataDroid.Configuration;

namespace TrackDataDroid.Views
{
    public class GpsPage : ContentPage
    {
        public GpsPage()
        {
            Content = new StackLayout();
        }
    }
}
