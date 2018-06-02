using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhantasmaMail.ViewModels;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletView : ContentPage
    {
        public WalletView()
        {
            InitializeComponent();
        }



        private async void PullToRefresh_Refreshing(object sender, EventArgs args)
        {
            assetList.IsRefreshing = true;
            await Task.Delay(2000);

            if (BindingContext is WalletTabViewModel vm) //await vm.RefreshExecute();
                assetList.IsRefreshing = false;
        }
    }
}