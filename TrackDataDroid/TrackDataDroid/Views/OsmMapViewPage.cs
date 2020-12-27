using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;
using CoordinateDataModels;
using Xamarin.Essentials;
using System;
using Mapsui.Layers;
using TrackDataDroid.Repositories;
using TrackDataDroid.Services;
using TrackDataDroid.Configuration;

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
                    ViewUtility.WrapViewInFrame(GetMapControlPanel()),
                    ViewUtility.WrapViewInFrame(await _viewModel.GetMapViewAsync())
                }
            }.Bind(StackLayout.OrientationProperty, nameof(_viewModel.CurrentStackOrientation));
            return stackLayout;
        }

        private View GetMapControlPanel()
        {
            var refreshView = new RefreshView().Content = new StackLayout
            {
                Padding = 5,
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
                                        new ColumnDefinition { Width = new GridLength(6,GridUnitType.Star)}, 
                                        new ColumnDefinition { Width = new GridLength(3,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(3,GridUnitType.Star)}
                                    },
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 5,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=12}
                                        .Bind(Label.TextProperty, $"{nameof(ILayer.Name)}")
                                        .Style(StyleRepository.DataItemTitleStyle)
                                        .Row(0).Column(0).CenterVertical(),
                                    new ImageButton {Source = ImageUtility.GetFontImageSource(IconNameConstants.Crosshairs) }
                                        .Style(StyleRepository.ImageButtonStyle)
                                        .Row(0).Column(1).CenterVertical(),
                                    new Switch()
                                        .Bind(Switch.IsToggledProperty, nameof(ILayer.Enabled))
                                        .Row(0).Column(2).CenterVertical()
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
   
