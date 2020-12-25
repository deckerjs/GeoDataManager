using System;
using System.Collections.Generic;
using System.Text;
using TrackDataDroid.Configuration;
using TrackDataDroid.Repositories;
using Xamarin.Forms;

namespace TrackDataDroid.Services
{
    public static class ImageUtility
    {
        public enum DefaultImageSize
        {
            Small=12,
            Medium=36,
            Large=44
        }

        public static FontImageSource GetFontImageSource(
            string iconName, 
            DefaultImageSize size = DefaultImageSize.Small, 
            string color = StyleRepository.Primary_Color)
        {
            var imageSource = new FontImageSource
            {
                Color = Color.FromHex(color),
                FontFamily = FontIconFamily.FA_Solid,
                Size = (double)size,
                Glyph = iconName
            };
            return imageSource;
        }


    }
}
