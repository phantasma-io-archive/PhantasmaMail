using FFImageLoading.Transformations;
using PhantasmaMail.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletAssetCell : Grid
    {
        public WalletAssetCell()
        {
            InitializeComponent();
            if (BindingContext is AssetModel asset)
            {
                arrow.Transformations.Add(asset.FiatChangePercentage >= 0
                    ? new TintTransformation((string) Application.Current.Resources["TintUpArrowColor"])
                    : new TintTransformation((string) Application.Current.Resources["TintDownArrowColor"]));
            }
        }
    }
}