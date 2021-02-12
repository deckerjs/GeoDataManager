using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;
using CoordinateDataModels;
using Xamarin.Essentials;
using System;
using TrackDataDroid.Configuration;
using TrackDataDroid.Repositories;
using TrackDataDroid.Services;

namespace TrackDataDroid.Services
{
    public static class ViewUtility
    {
        public static View WrapViewInFrame(View view)
        {
            var frame = new Frame
            {
                Content = view
            };
            return frame;
        }
    }
}
