using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class FiatValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "-";
            var fiat = (decimal)value;
            if (fiat <= 0) return "-";
            return fiat.ToString("$0.##");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
