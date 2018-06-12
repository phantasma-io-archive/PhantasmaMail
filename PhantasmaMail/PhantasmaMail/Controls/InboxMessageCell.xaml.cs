using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InboxMessageCell : Grid
    {
        public InboxMessageCell()
        {
            InitializeComponent();
            //customFrame.On<iOS>().UseBlurEffect(BlurEffectStyle.Dark);
            customFrame.On<Xamarin.Forms.PlatformConfiguration.Android>().SetElevation(20);
        }
    }
}