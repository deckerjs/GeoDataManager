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
using Microsoft.Extensions.Logging;

namespace TrackDataDroid.Views
{
    public class MapUrlLayerPage : ContentPage
    {
        private readonly MapViewModel _viewModel;
        private readonly ILogger<MapUrlLayerPage> _logger;
        private bool _initialized;
        private FontImageSource _refreshIconImageSrc;
        private FontImageSource _addIconImageSrc;
        private FontImageSource _removeIconImageSrc;

        public MapUrlLayerPage(MapViewModel viewModel, ILogger<MapUrlLayerPage> logger)
        {
            _viewModel = viewModel;
            _logger = logger;
            BindingContext = _viewModel;

            InitializeIconImages();
        }

        private void InitializeIconImages()
        {
            _refreshIconImageSrc = ImageUtility.GetFontImageSource(IconNameConstants.Sync);
            _addIconImageSrc = ImageUtility.GetFontImageSource(IconNameConstants.PlusCircle);
            _removeIconImageSrc = ImageUtility.GetFontImageSource(IconNameConstants.MinusCircle);
        }

        protected override async void OnAppearing()
        {
            try
            {
                if (!_initialized)
                {
                    Content = GetPageContent();
                    _initialized = true;
                }
                base.OnAppearing();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private View GetPageContent()
        {
            var stackLayout = new StackLayout
            {
                Padding = 5,
                Children =
                {
                    ViewUtility.WrapViewInFrame(GetAddUrlLayerPanel()),
                    ViewUtility.WrapViewInFrame(GetUrlLayersPanel())
                }
            };

            return stackLayout;
        }

        private View GetAddUrlLayerPanel()
        {
            return new Grid
            {
                RowDefinitions =
                        {
                            new RowDefinition { Height = GridLength.Auto}
                        },
                ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                            new ColumnDefinition { Width = new GridLength(5,GridUnitType.Star)},
                            new ColumnDefinition { Width = new GridLength(6,GridUnitType.Star)},
                        },
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Padding = 1,
                Children =
                        {
                            new Entry
                                { 
                                    Placeholder = "Layer Name",
                                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing
                                }
                                .Bind($"{nameof(_viewModel.LayerViewModelEntry)}.{nameof(_viewModel.LayerViewModelEntry.Name)}")
                                .Row(0).Column(0),
                            new Entry
                                { 
                                    Placeholder = "Layer Url",
                                    ClearButtonVisibility = ClearButtonVisibility.WhileEditing
                                }
                                .Bind($"{nameof(_viewModel.LayerViewModelEntry)}.{nameof(_viewModel.LayerViewModelEntry.LayerData)}")
                                .Row(0).Column(1),
                            new Button {HeightRequest = 20, ImageSource = _addIconImageSrc}
                                .Style(StyleRepository.ComandButtonStyle)
                                .BindCommand(nameof(_viewModel.AddUrlLayerCommand),_viewModel,nameof(_viewModel.LayerViewModelEntry))
                                .Row(0).Column(2)
                        }
            };
        }

        private View GetUrlLayersPanel()
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
                                        new ColumnDefinition { Width = new GridLength(3,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)},
                                        new ColumnDefinition { Width = new GridLength(6,GridUnitType.Star)}
                                    },
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(LayerViewModel<string>.Description)}")
                                        .Style(StyleRepository.DataItemTitleStyle)
                                        .Row(0).Column(0).CenterVertical(),
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(LayerViewModel<string>.LayerData)}")
                                        .Style(StyleRepository.DataItemValueStyle)
                                        .Row(0).Column(1).CenterVertical(),
                                    new Button {ImageSource=_removeIconImageSrc}
                                        .BindCommand(nameof(_viewModel.RemoveUrlLayerCommand),_viewModel,".")
                                        .Row(0).Column(2).CenterVertical(),
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.MapUrlLayers))
                }
            };
            return refreshView;
        }

    }
}

