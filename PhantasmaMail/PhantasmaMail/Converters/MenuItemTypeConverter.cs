using System;
using System.Globalization;
using PhantasmaMail.Models.UI;
using Xamarin.Forms;

namespace PhantasmaMail.Converters
{
    public class MenuItemTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var menuItemType = (MenuItemType)value;

            var platform = Device.RuntimePlatform == Device.UWP;

            switch (menuItemType)
            {
                case MenuItemType.Inbox:
                    return platform ? "Assets/inbox.svg" : "inbox.svg";
                case MenuItemType.Compose:
                    return platform ? "Assets/file_text.svg" : "file_text.svg";
                case MenuItemType.Sent:
                    return platform ? "Assets/send.svg" : "send.svg";
                case MenuItemType.Trash:
                    return platform ? "Assets/trash.svg" : "trash.svg";
                case MenuItemType.Wallet:
                    return platform ? "Assets/tablet.svg" : "tablet.svg";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
