using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;
using CoordinateDataModels;
using Xamarin.Essentials;
using System;
using TrackDataDroid.Configuration;

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

        //todo: put these somewhere like a repo or service
        public static Style<Label> DataItemTitleStyle => new Style<Label>(
        (Label.TextColorProperty, "#249C47"),
        (Label.FontAttributesProperty, FontAttributes.Bold));

        public static Style<Label> DataItemValueStyle => new Style<Label>(
            (Label.TextColorProperty, "#FFFFFF"));

        //public static Style<Button> ComandButtonStyle => new Style<Button>(
        //(Button.TextColorProperty, "#249C47"),
        //(Button.FontAttributesProperty, FontAttributes.Bold));
        
        public static Style<Button> ComandButtonStyle => new Style<Button>(
        (Button.TextColorProperty, "#249C47"),
        (Button.FontFamilyProperty, FontIconFamily.FA_Solid));

        private void InitializeIconImages()
        {

            _refreshIconImageSrc = new FontImageSource
            {
                FontFamily = FontIconFamily.FA_Solid,
                Size = 14,
                Glyph = IconNameConstants.Sync
            };
            
            _addIconImageSrc = new FontImageSource
            {
                FontFamily = FontIconFamily.FA_Solid,
                Size = 14,
                Glyph = IconNameConstants.PlusCircle
            };

            _removeIconImageSrc = new FontImageSource
            {
                FontFamily = FontIconFamily.FA_Solid,
                Size = 14,
                Glyph = IconNameConstants.MinusCircle
            };
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
                Children =
                {
                    GetAvailableTracksPanel(),
                    GetLayerManagemenPanel()
                }
            }.Bind(StackLayout.OrientationProperty, nameof(_viewModel.CurrentStackOrientation));
            return stackLayout;
        }

        private View GetAvailableTracksPanel()
        {
            var refreshView = new RefreshView().Content = new StackLayout
            {
                Margin = 2,
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
                                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)}
                                },
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 1,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(TrackSummaryViewModel.CoordinateData)}.{nameof(CoordinateDataSummary.Description)}")
                                        .Style(DataItemValueStyle)
                                        .Row(0).Column(0).CenterVertical(),
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(TrackSummaryViewModel.CoordinateData)}.{nameof(CoordinateDataSummary.DataItemCount)}")
                                        .Style(DataItemValueStyle)
                                        .Row(0).Column(1).CenterVertical(),
                                    new Button { ImageSource=_addIconImageSrc}
                                        .BindCommand(nameof(_viewModel.LoadTrackCommand),_viewModel)
                                        .Style(ComandButtonStyle)
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
                            new ColumnDefinition { Width = new GridLength(5,GridUnitType.Star)}
                        },
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = 1,
                        Margin = 1,
                        Children =
                        {
                            //new Button {Text = IconNameConstants.Sync, HeightRequest = 40, FontSize = 12, ImageSource = _refreshIconImageSrc}
                            new Button {HeightRequest = 40, ImageSource = _refreshIconImageSrc}
                                .Style(ComandButtonStyle)
                                .BindCommand(nameof(_viewModel.LoadAvailableTracksCommand))
                                .Row(0).Column(0)
                        }
                    }
                }
            };
            //.Bind(StackLayout.HeightRequestProperty, nameof(_viewModel.Section2Height))
            //.Bind(StackLayout.WidthRequestProperty, nameof(_viewModel.Section2Width));
            return refreshView;
        }

        private View GetLayerManagemenPanel()
        {
            var refreshView = new RefreshView().Content = new StackLayout
            {
                Margin = 2,
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
                                        new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star)}
                                    },
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 1,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(LayerViewModel<CoordinateData>.LayerData)}.{nameof(CoordinateData.Description)}")
                                        .Style(DataItemValueStyle)
                                        .Row(0).Column(0).CenterVertical(),
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(LayerViewModel<CoordinateData>.LayerData)}.{nameof(CoordinateData.Data)}.{nameof(CoordinateData.Data.Count)}")
                                        .Style(DataItemValueStyle)
                                        .Row(0).Column(1).CenterVertical(),
                                    new Button {ImageSource=_removeIconImageSrc}
                                        .BindCommand(nameof(_viewModel.RemoveLoadedTrackCommand),_viewModel)
                                        .Row(0).Column(2).CenterVertical(),
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.AvailableTrackLayers))
                }
            };
            //.Bind(StackLayout.HeightRequestProperty, nameof(_viewModel.Section1Height))
            //.Bind(StackLayout.WidthRequestProperty, nameof(_viewModel.Section1Width));
            return refreshView;
        }

    }
}
