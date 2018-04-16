using System;
using System.Globalization;
using NeoModules.Core;
using NeoModules.NEP6.Models;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class ScriptHashToAddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var scriptHash = value as UInt160;
            return Wallet.ToAddress(scriptHash);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}