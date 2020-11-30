using Xamarin.Forms;
using TrackDataDroid.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms.Markup;

namespace TrackDataDroid.Views
{
    public class OsmMapViewPage : ContentPage
    {
        private readonly MapViewModel _viewModel;

        public OsmMapViewPage(MapViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {            
            Content = await GetPageContent();
            base.OnAppearing();
        }

        private async Task<View> GetPageContent()
        {
            var stackLayout = new StackLayout
            {
                Children =
                {
                await _viewModel.GetMapViewAsync()
                }
            };
            return stackLayout;
        }

        private View GetMapControlPanel()
        {
            var stackLayout = new StackLayout
            {
                Children =
                {
                new Button {Text = "Reload"}.BindCommand(nameof(_viewModel.LoadItemsCommand)),                    
                    new CollectionView()
                    {
                        ItemTemplate = new DataTemplate(() =>
                        {
                            return new StackLayout
                                {
                                    Padding = 10,
                                    Children =
                                        {
                                    new Label{Text="placeholder 1"},
                                    new Label{Text="placeholder 2"},
                                            //new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=16}
                                            //    .Bind(Label.TextProperty, nameof(SensorValueItem.Name))
                                            //    .Style(DataItemTitleStyle),
                                            //new Label{LineBreakMode = LineBreakMode.NoWrap, FontSize=14}
                                            //    .Bind(Label.TextProperty, nameof(SensorValueItem.Value))
                                            //    .Style(DataItemValueStyle),
                                        }
                                };
                        })
                    }.Bind(CollectionView.ItemsSourceProperty, nameof(_viewModel.MapDataItems))
                }
            };
            return stackLayout;

        }

    }
}
   
