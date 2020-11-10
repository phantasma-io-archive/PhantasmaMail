using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class DraftMissingFieldsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)//todo localization
        {
            var param = parameter as string;
            if (value == null)
            {
                if (param != null && param.Equals("1"))
                {
                    return "(without subject)";
                }
                if (param != null && param.Equals("2"))
                {
                    return "This draft does not have content.";
                }
                if (param != null && param.Equals("3"))
                {
                    return "(without destination)";
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
