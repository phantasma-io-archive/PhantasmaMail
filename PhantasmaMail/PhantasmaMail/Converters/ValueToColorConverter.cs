using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Color.Black;

            if (value is decimal val && val < 0)
            {
                var x = (string)Application.Current.Resources["TintDownArrowColor"];
                return Color.FromHex(x);
            }
            return Color.FromHex((string)Application.Current.Resources["TintUpArrowColor"]);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
