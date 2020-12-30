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
    public class MapDataPage : ContentPage
    {
        private readonly MapViewModel _viewModel;
        private bool _initialized;
        private FontImageSource _refreshIconImageSrc;
        private FontImageSource _addIconImageSrc;
        private FontImageSource _removeIconImageSrc;

        public MapDataPage(MapViewModel viewModel)
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
                _viewModel.ReadyToLoadTracks = true;
                await _viewModel.LoadAvailableTracks();
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
                    ViewUtility.WrapViewInFrame(GetAvailableTracksPanel()),
                    ViewUtility.WrapViewInFrame(GetLayerManagemenPanel())
                }
            }
             .Bind(StackLayout.OrientationProperty, nameof(_viewModel.CurrentStackOrientation));
            return stackLayout;
        }

        private View GetAvailableTracksPanel()
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
                                        .Bind(Label.TextProperty, $"{nameof(TrackSummaryViewModel.CoordinateData)}.{nameof(CoordinateDataSummary.Description)}")
                                        .Style(StyleRepository.DataItemTitleStyle)
                                        .Row(0).Column(0).CenterVertical(),
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(TrackSummaryViewModel.CoordinateData)}.{nameof(CoordinateDataSummary.DataItemCount)}")
                                        .Style(StyleRepository.DataItemValueStyle)
                                        .Row(0).Column(1).CenterVertical(),
                                    new Button { ImageSource=_addIconImageSrc}
                                        .BindCommand(nameof(_viewModel.LoadTrackCommand),_viewModel,".")
                                        .Style(StyleRepository.ComandButtonStyle)
                                        .Row(0).Column(2).CenterVertical()
                            }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.AvailableCoordinateData)),

                    new Grid
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
                            new Button {HeightRequest = 20, ImageSource = _refreshIconImageSrc}
                                .Style(StyleRepository.ComandButtonStyle)
                                .BindCommand(nameof(_viewModel.LoadAvailableTracksCommand))
                                .Row(0).Column(0)
                        }
                    }
                }
            };
            return refreshView;
        }

        private View GetLayerManagemenPanel()
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
                                        .Bind(Label.TextProperty, $"{nameof(LayerViewModel<CoordinateData>.LayerData)}.{nameof(CoordinateData.Description)}")
                                        .Style(StyleRepository.DataItemTitleStyle)
                                        .Row(0).Column(0).CenterVertical(),
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(LayerViewModel<CoordinateData>.LayerData)}.{nameof(CoordinateData.Data)}.{nameof(CoordinateData.Data.Count)}")
                                        .Style(StyleRepository.DataItemValueStyle)
                                        .Row(0).Column(1).CenterVertical(),
                                    new Button {ImageSource=_removeIconImageSrc}
                                        .BindCommand(nameof(_viewModel.RemoveLoadedTrackCommand),_viewModel,".")
                                        .Row(0).Column(2).CenterVertical(),
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.AvailableTrackLayers))
                }
            };
            return refreshView;
        }

    }
}
