using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sensortest.Models;
using sensortest.Services;
using sensortest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
//using Xamarin.Forms;
//using Xamarin.Forms.Xaml;

namespace sensortest.Views
{
    public partial class SettingItemsPage : ContentPage
    {
        private ISensorValuesViewModel _viewModel;
        public SettingItemsPage()
        {
            _viewModel = App.Host.Services.GetRequiredService<ISensorValuesViewModel>();
            BindingContext = _viewModel;
            Content = GetContent();
        }

        private View GetContent()
        {
            var refreshView = new RefreshView().Content = new StackLayout
            {
                Children =
                {
                    new Label {Text = "Sensor Items" , VerticalOptions=LayoutOptions.CenterAndExpand, HorizontalOptions=LayoutOptions.CenterAndExpand },
                    new Button {Text = "Reload"}.BindCommand(nameof(_viewModel.LoadItemsCommand)),
                    new Button {Text = "Open Map"}.BindCommand(nameof(_viewModel.OpenMapCommand)),
                    new CollectionView()
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            return new StackLayout
                                {
                                    Padding = 10,
                                    Children =
                                        {
                                            new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=16}.Bind(Label.TextProperty, nameof(SensorValueItem.Name)),
                                            new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=14}.Bind(Label.TextProperty, nameof(SensorValueItem.Value)),
                                        }
                                };
                        })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.Items))
                }
            };

            //doesnt like this, no error given
            //.Bind(RefreshView.IsRefreshingProperty, nameof(_viewModel.IsLoading),BindingMode.TwoWay);

            return refreshView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task viewModelInitTask = _viewModel.InitializeAsync();
        }
    }
}