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

namespace TrackDataDroid.Views
{
    public class MapFileLayerPage : ContentPage
    {
        private readonly MapViewModel _viewModel;
        private bool _initialized;
        private FontImageSource _refreshIconImageSrc;
        private FontImageSource _addIconImageSrc;
        private FontImageSource _removeIconImageSrc;

        public MapFileLayerPage(MapViewModel viewModel)
        {
            _viewModel = viewModel;
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
            if (!_initialized)
            {
                Content = GetPageContent();
                _initialized = true;
            }
            base.OnAppearing();
        }

        private View GetPageContent()
        {
            var stackLayout = new StackLayout
            {
                Padding = 5,
                Children =
                {
                    ViewUtility.WrapViewInFrame(GetLoadLayerFilePanel()),
                    ViewUtility.WrapViewInFrame(GetLoadedLayersPanel())
                }
            };
            
            return stackLayout;
        }

        private View GetLoadLayerFilePanel()
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
                        // open file dialog
                            new Button {HeightRequest = 20, ImageSource = _addIconImageSrc}
                                .Style(StyleRepository.ComandButtonStyle)
                                .BindCommand(nameof(_viewModel.SelectFileCommand))
                                .Row(0).Column(0)
                        }
            };            
        }

        private View GetLoadedLayersPanel()
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
                                        .BindCommand(nameof(_viewModel.RemoveMBTileFileLayerCommand),_viewModel,".")
                                        .Row(0).Column(2).CenterVertical(),
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.MapFileLayers))
                }
            };
            return refreshView;
        }

    }
}
