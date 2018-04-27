using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
   public class LoginTextBooleanToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return Color.FromHex("#8AA8A3");

            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
