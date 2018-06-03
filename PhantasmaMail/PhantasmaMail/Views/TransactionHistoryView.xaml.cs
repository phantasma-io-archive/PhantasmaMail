using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhantasmaMail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionHistoryView : ContentPage
	{
		public TransactionHistoryView ()
		{
			InitializeComponent ();
		}

	    //private async void PullToRefresh_Refreshing(object sender, EventArgs e)
	    //{
	    //    transactionsList.IsRefreshing = true;
	    //    await Task.Delay(2000);

	    //    if (BindingContext is WalletTabViewModel vm) //todo await vm.RefreshExecute();
	    //        transactionsList.IsRefreshing = false;
     //   }
	}
}