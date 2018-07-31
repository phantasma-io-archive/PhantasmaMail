using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using PhantasmaMail.UWP.Extensions;
using PhantasmaMail.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Xfx;

[assembly: ExportRenderer(typeof(XfxEntry), typeof(XfxEntryRenderer))]
namespace PhantasmaMail.UWP.Renderers
{
    public class XfxEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderThickness = new Windows.UI.Xaml.Thickness(1);
                Control.BorderBrush = new SolidColorBrush(Colors.White);

                Control.Background = new SolidColorBrush(e.NewElement.BackgroundColor.ToUwp());
                Control.BackgroundFocusBrush = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
