using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class UtcToLocalTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var date = value is DateTime time ? time : new DateTime();
                return date.ToLocalTime();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
