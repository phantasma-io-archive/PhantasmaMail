using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class LoginButtonBooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value) return Color.White;

            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}