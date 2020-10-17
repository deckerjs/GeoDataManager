using System;
using System.Collections.Generic;
using sensortest.ViewModels;
using sensortest.Views;
using Xamarin.Forms;

namespace sensortest
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
