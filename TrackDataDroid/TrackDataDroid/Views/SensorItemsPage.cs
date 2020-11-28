using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrackDataDroid.Models;
using TrackDataDroid.Services;
using TrackDataDroid.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace TrackDataDroid.Views
{
    public partial class SensorItemsPage : ContentPage
    {
        private ISensorValuesViewModel _viewModel;
        public SensorItemsPage(ISensorValuesViewModel viewModel)
        {
            _viewModel = viewModel;            
            BindingContext = _viewModel;
            Title = "Sensor Items";
            Content = GetContent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task viewModelInitTask = _viewModel.InitializeAsync();
        }
    
        private View GetContent()
        {
            var refreshView = new RefreshView().Content = new StackLayout
            {
                Children =
                {                    
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
                                            new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=16}
                                                .Bind(Label.TextProperty, nameof(SensorValueItem.Name))
                                                .Style(DataItemTitleStyle),
                                            new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=14}
                                                .Bind(Label.TextProperty, nameof(SensorValueItem.Value))
                                                .Style(DataItemValueStyle),
                                        }
                                };
                        })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.Items))
                }
            };

            return refreshView;
        }

        // explicit style overrides
        public static Style<Label> DataItemTitleStyle => new Style<Label>(
                (Label.TextColorProperty, "#249C47"),
                (Label.FontAttributesProperty, FontAttributes.Bold));

        public static Style<Label> DataItemValueStyle => new Style<Label>(
            (Label.TextColorProperty, "#FFFFFF"));

        // implicit style overrides
        public static ResourceDictionary GetResources()
        {
            return new ResourceDictionary() { };
        }
    }
}