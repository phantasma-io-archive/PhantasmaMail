using System;
using System.Threading.Tasks;
using PhantasmaMail.Models;
using PhantasmaMail.Utils;
using PhantasmaMail.ViewModels;
using Syncfusion.DataSource;
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
		    sentList.DataSource.GroupDescriptors.Add(new GroupDescriptor()
		    {
		        PropertyName = "Date",
		        KeySelector = (obj1) =>
		        {
		            var item = (obj1 as Message);
		            return item.GroupDate;
		        },
		        Comparer = new MessageDateGroupComparer()
		    });
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