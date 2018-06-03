using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PhantasmaMail.Models;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class TransactionToSenderReceiverConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TransactionModel tx)
            {
                if (tx.ImagePath == "ic_receive.png") return tx.FromAddress;
                if (tx.ImagePath == "ic_send.png") return tx.ToAddress;
                return tx.ToAddress;
            }
            return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
