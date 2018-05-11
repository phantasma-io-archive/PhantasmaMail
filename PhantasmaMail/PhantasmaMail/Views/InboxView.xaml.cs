using System;
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

	    protected override void OnAppearing()
	    {
	        pullToRefreshList.ForceLayout(); //todo: date label bug
	    }

	    private async void PullToRefresh_Refreshing(object sender, EventArgs args)
	    {
	        pullToRefreshList.IsRefreshing = true;
	        await Task.Delay(2000);

	        if (BindingContext is InboxViewModel vm) await vm.RefreshExecute();
	        pullToRefreshList.IsRefreshing = false;
	    }
    }
}