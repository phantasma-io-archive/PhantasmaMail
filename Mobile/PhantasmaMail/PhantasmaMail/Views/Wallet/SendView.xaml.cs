using System;
using PhantasmaMail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace PhantasmaMail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendView : ContentPage
    {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;

        public SendView()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;
                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (this.BindingContext is WalletTabViewModel vm)
                    {
                        vm.ToAddress = result.Text;
                    }
                    await Navigation.PopAsync();
                });
            };
            await Navigation.PushAsync(scanPage);
        }
    }
}