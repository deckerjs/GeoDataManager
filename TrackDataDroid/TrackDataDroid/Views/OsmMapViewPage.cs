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

        //OnBindingContextChanged

        protected override async void OnAppearing()
        {
            if (!_initialized)
            {
                Content = await GetPageContent();
                //await _viewModel.LoadAvailableTracks();
                _initialized = true;
            }

            base.OnAppearing();
        }

        private async Task<View> GetPageContent()
        {
            var stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.Horizontal,
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
                //new Button {Text = "Reload"}.BindCommand(nameof(_viewModel.LoadAvailableTracksCommand)),                    
                new CollectionView()
                {
                    ItemTemplate = new DataTemplate(() =>
                    {
                        return new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 1,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                        .Bind(Label.TextProperty, $"{nameof(ILayer.Name)}")
                                        .Style(DataItemValueStyle),                                    
                                    //new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=10}
                                    //    .Bind(Label.TextProperty, $"{nameof(ILayer.Enabled)}")
                                    //    .Style(DataItemValueStyle),
                                    //new Button {Text = "+", WidthRequest=15, HeightRequest=15, FontSize=14}.BindCommand(nameof(_viewModel.LoadTrackCommand),_viewModel,$"CoordinateData.{nameof(CoordinateDataSummary.ID)}"),
                                    new CheckBox().Bind(CheckBox.IsCheckedProperty, nameof(ILayer.Enabled)).Style(DataItemCheckStyle)
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
   
