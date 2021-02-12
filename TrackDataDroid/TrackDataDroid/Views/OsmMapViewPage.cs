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
using Microsoft.Extensions.Logging;

namespace TrackDataDroid.Views
{
    public class OsmMapViewPage : ContentPage
    {
        private readonly MapViewModel _viewModel;
        private readonly ILogger<OsmMapViewPage> _logger;
        private bool _initialized;

        public OsmMapViewPage(MapViewModel viewModel, ILogger<OsmMapViewPage> logger)
        {
            _viewModel = viewModel;
            _logger = logger;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            try
            {
                if (!_initialized)
                {
                    Content = await GetPageContent();
                    _initialized = true;
                }
                base.OnAppearing();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
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
                                RowDefinitions =
                                {
                                    new RowDefinition { Height = new GridLength(20)},
                                    new RowDefinition { Height = new GridLength(20)}
                                },
                                ColumnDefinitions = 
                                    {
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},                                        new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)}
                                    },
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 5,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=12}
                                        .Bind(Label.TextProperty, $"{nameof(ILayer.Name)}")
                                        .Style(StyleRepository.DataItemTitleStyle)
                                        .Row(0).Column(0).ColumnSpan(9).CenterVertical(),
                                    new Switch()
                                        .Bind(Switch.IsToggledProperty, nameof(ILayer.Enabled))
                                        .Height(40)
                                        .Row(0).RowSpan(2).Column(10).ColumnSpan(3),
                                    new ImageButton {Source = ImageUtility.GetFontImageSource(IconNameConstants.Crosshairs)}
                                        .Style(StyleRepository.ImageButtonStyle)
                                        .Row(1).Column(0).ColumnSpan(2).CenterVertical()
                                        .BindCommand(nameof(_viewModel.NavToLayerCenterCommand),_viewModel,"."),
                                    new ImageButton {Source = ImageUtility.GetFontImageSource(IconNameConstants.ArrowDown)}
                                        .Style(StyleRepository.ImageButtonStyle)
                                        .Row(1).Column(2).ColumnSpan(2).CenterVertical()
                                        .BindCommand(nameof(_viewModel.MoveLayerUpCommand),_viewModel,"."),
                                    new ImageButton {Source = ImageUtility.GetFontImageSource(IconNameConstants.ArrowUp)}
                                        .Style(StyleRepository.ImageButtonStyle)
                                        .Row(1).Column(4).ColumnSpan(2).CenterVertical()
                                        .BindCommand(nameof(_viewModel.MoveLayerDownCommand),_viewModel,".")
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, $"{nameof(_viewModel.MapLayersFiltered)}")
                }
            }
            .Bind(StackLayout.HeightRequestProperty, nameof(_viewModel.Section2Height))
            .Bind(StackLayout.WidthRequestProperty, nameof(_viewModel.Section2Width));
            return refreshView;

        }

    }
}
   
