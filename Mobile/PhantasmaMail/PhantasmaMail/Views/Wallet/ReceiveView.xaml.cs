using System;
using PhantasmaMail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace PhantasmaMail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiveView : ContentPage
    {
        private ZXingBarcodeImageView _barcode;
        private bool firstRun = true;

        public ReceiveView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (!firstRun) return;
            try
            {
                if (BindingContext is WalletTabViewModel vm)
                {
                    _barcode = new ZXingBarcodeImageView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        BarcodeFormat = ZXing.BarcodeFormat.QR_CODE,
                        BarcodeOptions =
                        {
                            Width = 250,
                            Height = 250
                        },
                        BarcodeValue = vm.UserAddress,
                    };
                    QRView.Content = _barcode;
                    this.InvalidateMeasure();
                }    
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            firstRun = false;
        }
    }
}