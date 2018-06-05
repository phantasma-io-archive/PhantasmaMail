using System;
using System.Globalization;
using PhantasmaMail.Models;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class AssetAmountSymbolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return "-";
                case AssetModel asset:
                    var result = asset.Amount.ToString("0.########") + " " + asset.TokenDetails.Symbol;
                    return result;
                default:
                    return "-";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
