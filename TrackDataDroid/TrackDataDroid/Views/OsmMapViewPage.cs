using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;
using CoordinateDataModels;
using Xamarin.Essentials;
using System;
using Mapsui.Layers;

namespace TrackDataDroid.Views
{
    public class OsmMapViewPage : ContentPage
    {
        private readonly MapViewModel _viewModel;

        private bool _initialized;

        public OsmMapViewPage(MapViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        //todo: put these somewhere like a repo or service
        public static Style<Label> DataItemTitleStyle => new Style<Label>(
        (Label.TextColorProperty, "#249C47"),
        (Label.FontAttributesProperty, FontAttributes.Bold));

        public static Style<Label> DataItemValueStyle => new Style<Label>(
            (Label.TextColorProperty, "#FFFFFF"));
        
        public static Style<CheckBox> DataItemCheckStyle => new Style<CheckBox>(
            (CheckBox.ColorProperty, "#FFFFFF"));

        protected override async void OnAppearing()
        {
            if (!_initialized)
            {
                Content = await GetPageContent();
                _initialized = true;
            }

            base.OnAppearing();
        }

        private async Task<View> GetPageContent()
        {
            var stackLayout = new StackLayout
            {
                Children =
                {
                    GetMapControlPanel(),
                    await _viewModel.GetMapViewAsync()
                }
            }.Bind(StackLayout.OrientationProperty, nameof(_viewModel.CurrentStackOrientation));
            return stackLayout;
        }

        private View GetMapControlPanel()
        {

            var refreshView = new RefreshView().Content = new StackLayout
            {
                Children =
                {
                new CollectionView()
                {
                    ItemTemplate = new DataTemplate(() =>
                    {
                        return new Grid
                            {
                                ColumnDefinitions = 
                                    {
                                        new ColumnDefinition { Width = new GridLength(5,GridUnitType.Star)}, 
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)} 
                                    },
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 1,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(ILayer.Name)}")
                                        .Style(DataItemValueStyle)
                                        .Row(0).Column(0).CenterVertical(),                                    
                                    new CheckBox()
                                        .Bind(CheckBox.IsCheckedProperty, nameof(ILayer.Enabled))
                                        .Style(DataItemCheckStyle)
                                        .Row(0).Column(1).CenterVertical()
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, $"{nameof(_viewModel.Map)}.{nameof(_viewModel.Map.Layers)}")
                }
            }
            .Bind(StackLayout.HeightRequestProperty, nameof(_viewModel.Section2Height))
            .Bind(StackLayout.WidthRequestProperty, nameof(_viewModel.Section2Width));
            return refreshView;

        }

    }
}
   
