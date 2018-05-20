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
	public partial class SentView : ContentPage
	{
		public SentView ()
		{
			InitializeComponent ();
		}

	    private async void PullToRefresh_Refreshing(object sender, EventArgs args)
	    {
	        pullToRefreshList.IsRefreshing = true;
	        await Task.Delay(2000);

	        if (BindingContext is SentViewModel vm) await vm.RefreshExecute();
	        pullToRefreshList.IsRefreshing = false;
	    }
    }
}