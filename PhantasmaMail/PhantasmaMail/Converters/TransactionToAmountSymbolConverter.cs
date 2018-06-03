using System;
using System.Globalization;
using PhantasmaMail.Models;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class TransactionToAmountSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is TransactionModel tx)
                {
                    var str = "Amount: " + tx.Amount + " " + tx.Symbol;//todo resources
                    return str;
                }

            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
