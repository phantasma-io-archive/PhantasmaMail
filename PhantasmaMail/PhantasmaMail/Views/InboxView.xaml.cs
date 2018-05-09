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
	public partial class InboxView : ContentPage
	{
		public InboxView ()
		{
			InitializeComponent ();
		}

	    private async void PullToRefresh_Refreshing(object sender, EventArgs args)
	    {
	        pullToRefresh.IsRefreshing = true;
	        await Task.Delay(2000);

	        if (BindingContext is InboxViewModel vm) await vm.RefreshExecute();
	        pullToRefresh.IsRefreshing = false;
	    }
    }
}