using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class AssetValueChangeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var platform = Device.RuntimePlatform == Device.UWP;
            if (value == null) return platform ? "Assets/uppricearrow.svg" : "uppricearrow.svg";
            if (value is decimal decValue && decValue < 0)
            {
                return platform ? "Assets/downpricearrow.svg" : "downpricearrow.svg";
            }
            return platform ? "Assets/uppricearrow.svg" : "uppricearrow.svg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
