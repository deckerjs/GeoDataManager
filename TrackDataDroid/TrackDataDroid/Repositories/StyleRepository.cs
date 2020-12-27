using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TrackDataDroid.Services;
using TrackDataDroid.Views;
using TrackDataDroid.Models;
using TrackDataDroid.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xamarin.Forms.Markup;
using GeoStoreApi.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TrackDataDroid.Repositories;
using TrackDataDroid.Configuration;

namespace TrackDataDroid.Repositories
{
    public class StyleRepository
    {
        //private const string Primary_Text_Color = "#209DF7";
        //private const string Primary_Page_Background_Color = "#252526";
        //private const string Primary_Alt_Background_Color = "#333337";

        public const string Lt_blue = "#5695D8";
        public const string Med_blue = "#2763DB";
        public const string Dk_gray = "#404040";
        public const string Orange = "#FFA500";

        //private const string Primary_Color = "#B9CCA4";
        public const string Primary_Color = "#B2BE85";
        public const string Primary_Color_Shaded = "#51695C";
        public const string Primary_Color_Highlight = Orange;
        
        public const string Secondary_Color = "#209DF7";
        public const string Secondary_Color_Shaded = "#29324E";

        public const string Primary_White = "#C7D5E1";
        public const string Primary_DarkShade = "#252526";
        public const string Primary_Black = "#0D1110";





        //public static Style<Frame> OuterFrameStyle => new Style<Frame>(
        //    (Frame.BorderColorProperty, Primary_Color),
        //    (Frame.CornerRadiusProperty, 3),
        //    (Frame.PaddingProperty, 5)
        //    );

        public static Style<Label> DataItemTitleStyle => new Style<Label>(
            (Label.TextColorProperty, Primary_Color),
            (Label.FontAttributesProperty, FontAttributes.Bold));

        public static Style<Label> DataItemValueStyle => new Style<Label>(
            (Label.TextColorProperty, Secondary_Color));

        public static Style<Button> ComandButtonStyle => new Style<Button>(
            (Button.HeightRequestProperty, 20),
            (Button.TextColorProperty, Primary_Color),
            (Button.BorderColorProperty, Primary_Color),
            (Button.BorderWidthProperty, 1),
            (Button.CornerRadiusProperty, 3),
            (Button.BackgroundColorProperty, Primary_Color_Shaded),
            (Button.FontFamilyProperty, FontIconFamily.FA_Solid));

        public static Style<Button> AltComandButtonStyle => new Style<Button>(
            (Button.HeightRequestProperty, 20),
            (Button.TextColorProperty, Secondary_Color),
            (Button.BorderColorProperty, Secondary_Color),
            (Button.BorderWidthProperty, 1),
            (Button.CornerRadiusProperty, 3),
            (Button.BackgroundColorProperty, Secondary_Color_Shaded),
            (Button.FontFamilyProperty, FontIconFamily.FA_Solid));

        public static Style<ImageButton> ImageButtonStyle => new Style<ImageButton>(
                    (ImageButton.HeightRequestProperty, 20),
                    (ImageButton.BorderColorProperty, Primary_Color),
                    (ImageButton.BorderWidthProperty, 1),
                    (ImageButton.CornerRadiusProperty, 3),
                    (ImageButton.BackgroundColorProperty, Primary_Color_Shaded)
                    );

        public static Style<CheckBox> DataItemCheckStyle => new Style<CheckBox>(
            (CheckBox.ColorProperty, Primary_Color));
        
        public static Style<Switch> DataItemSwitchStyle => new Style<Switch>(            
            (Switch.ThumbColorProperty, Primary_Color),
            (Switch.OnColorProperty, Primary_Color_Shaded),
            (Switch.BackgroundColorProperty, Primary_DarkShade));

        public static ResourceDictionary DefaultStyle()
        {

            Style<Element> baseStyle = new Style<Element>(
             (Shell.BackgroundColorProperty, Color.Black),
             (Shell.ForegroundColorProperty, Primary_Color),
             (Shell.TitleColorProperty, Primary_Color),
             (Shell.DisabledColorProperty, Primary_DarkShade),
             (Shell.UnselectedColorProperty, Primary_Color_Shaded),
             (Shell.TabBarBackgroundColorProperty, Color.Black),
             (Shell.TabBarForegroundColorProperty, Primary_Color)
             );

            Style<Shell> shell = new Style<Shell>().BasedOn(baseStyle);
            Style<TabBar> tabBar = new Style<TabBar>().BasedOn(baseStyle);
            Style<FlyoutItem> flyoutItem = new Style<FlyoutItem>().BasedOn(baseStyle);

            Style<ContentPage> contentPage = new Style<ContentPage>(
                (ContentPage.BackgroundColorProperty, Primary_Black)
                ).ApplyToDerivedTypes(true);

            Style<Frame> OuterFrameStyle = new Style<Frame>(
                (Frame.BackgroundColorProperty, Primary_Black),
                (Frame.BorderColorProperty, Primary_Color),
                (Frame.CornerRadiusProperty, 3),
                (Frame.PaddingProperty, 5)
                );

            Style<Label> label = new Style<Label>(                
                (Label.TextColorProperty, Primary_Color));

            Style<CollectionView> collectionView = new Style<CollectionView>(
                (CollectionView.BackgroundColorProperty, Primary_Black));

            //Style<Button> button = new Style<Button>(
            //    (Button.BackgroundColorProperty, Primary_Alt_Background_Color),
            //    (Button.TextColorProperty, Primary_Text_Color),
            //    (Button.BorderColorProperty, "#404040"),
            //    (Button.BorderWidthProperty, 2)
            //    );

            Style<Button> ComandButtonStyle = new Style<Button>(
                        (Button.HeightRequestProperty, 20),
                        (Button.TextColorProperty, Primary_Color),
                        (Button.BorderColorProperty, Primary_Color),
                        (Button.BorderWidthProperty, 1),
                        (Button.CornerRadiusProperty, 3),
                        (Button.BackgroundColorProperty, Primary_Color_Shaded),
                        (Button.FontFamilyProperty, FontIconFamily.FA_Solid));

            return new ResourceDictionary()
                {
                    baseStyle,
                    shell,
                    tabBar,
                    flyoutItem,
                    contentPage,
                    OuterFrameStyle,
                    collectionView,
                    label,
                    ComandButtonStyle,
                    DataItemCheckStyle,
                    DataItemSwitchStyle
                };
        }

    }
}
