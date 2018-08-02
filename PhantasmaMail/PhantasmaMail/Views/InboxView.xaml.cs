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
    public partial class InboxView : ContentPage
    {
        private readonly InboxViewModel _vm;
        public InboxView()
        {
            InitializeComponent();
            inboxListView.DataSource.GroupDescriptors.Add(new GroupDescriptor()
            {
                PropertyName = "Date",
                KeySelector = (obj1) =>
                {
                    var item = (obj1 as Message);
                    return item.GroupDate;
                },
                Comparer = new MessageDateGroupComparer()
            });
            _vm = BindingContext as InboxViewModel;
        }

        protected override void OnAppearing()
        {
            pullToRefreshList.ForceLayout(); //todo: date label bug
        }

        private async void PullToRefresh_Refreshing(object sender, EventArgs args)
        {
            pullToRefreshList.IsRefreshing = true;
            await Task.Delay(2000);

            await _vm.RefreshExecute();
            pullToRefreshList.IsRefreshing = false;
        }
    }
}