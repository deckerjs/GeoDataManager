using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;
using CoordinateDataModels;
using Xamarin.Essentials;
using System;

namespace TrackDataDroid.Views
{
    public class OsmMapViewPage : ContentPage
    {
        private readonly MapViewModel _viewModel;

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



        protected override async void OnAppearing()
        {            
            Content = await GetPageContent();
            base.OnAppearing();
        }

        private async Task<View> GetPageContent()
        {
            var stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.Horizontal,
                Children =
                {
                    await GetMapControlPanel(),
                    await _viewModel.GetMapViewAsync()
                }
            }.Bind(StackLayout.OrientationProperty, nameof(_viewModel.CurrentStackOrientation));
            return stackLayout;
        }

        private async Task<View> GetMapControlPanel()
        {
            await _viewModel.LoadAvailableTracks();

            var refreshView = new RefreshView().Content = new StackLayout
            {
                Children =
                {
                new Button {Text = "Reload"}.BindCommand(nameof(_viewModel.LoadTrackCommand)),                    
                new CollectionView()
                {
                    ItemTemplate = new DataTemplate(() =>
                    {
                        return new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Padding = 1,
                                Children =
                                    {
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=16, Text=nameof(CoordinateDataSummary.ID)}                                            
                                        .Style(DataItemTitleStyle),
                                    new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=14}
                                        .Bind(Label.TextProperty, nameof(CoordinateDataSummary.ID))
                                        .Style(DataItemValueStyle)
                                    }
                            };
                    })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.AvailableCoordinateData))
                }
            };
            return refreshView;

        }

    }
}
   
