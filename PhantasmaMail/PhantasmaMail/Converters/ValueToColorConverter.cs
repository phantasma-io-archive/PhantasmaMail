using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class ValueToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Color.Black;
            if (value is string str && str.StartsWith("-"))
            {
                return Color.FromHex("#D0021B");
            }
            return Color.FromHex("#7ED321");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
