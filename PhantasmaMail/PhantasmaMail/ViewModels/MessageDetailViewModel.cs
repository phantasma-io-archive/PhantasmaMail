using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.JsonRpc.Client;
using PhantasmaMail.Models;
using PhantasmaMail.Resources;
using PhantasmaMail.Services.Db;
using PhantasmaMail.Utils;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class MessageDetailViewModel : ViewModelBase
    {
        private readonly IPhantasmaDb _db;

        public MessageDetailViewModel(IPhantasmaDb phantasmaDb)
        {
            _db = phantasmaDb;
        }

        public ICommand DeleteMessageCommand => new Command(async () => await DeleteMessageExecute());

        public ICommand ForwardCommand => new Command(async () => await ForwardExecute());

        public ICommand ReplyCommand => new Command(async () => await ReplyExecute());

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());

        public ICommand MoreCommand => new Command(async () => await MoreExecute());

        public ICommand OpenTxCommand => new Command(async () => await OpenTxExecute());

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is object[] data && data[0] is Message message && data[1] is bool fromInbox)
            {
                FromInbox = fromInbox;
                FromOrTo = FromInbox ? "From: " : "To: "; //todo move to resources
                SelectedMessage = message;

                DaysAgo = MessageUtils.CalculateDays(message.Date.ToLocalTime());
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task DeleteMessageExecute()
        {
            if (IsBusy) return;
            string tx = null;
            try
            {
                IsBusy = true;

                if (FromInbox)
                    tx = await PhantasmaService.RemoveInboxMessage(SelectedMessage.ID);
                else
                    tx = await PhantasmaService.RemoveOutboxMessage(SelectedMessage.ID);
            }
            catch (Exception ex)
            {
                if (ex is RpcClientUnknownException || ex is RpcClientTimeoutException) //todo switch error message
                {
                    AppSettings.ChangeRpcServer();
                }
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                IsBusy = false;
            }

            if (!string.IsNullOrEmpty(tx))
            {
                await _db.DeleteMessage(SelectedMessage.ToStoreMessage());
                await DialogService.ShowAlertAsync("Message will be deleted in the next block", "Success"); //todo move to resources
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                await DialogService.ShowAlertAsync(AppResource.Alert_SomethingWrong, AppResource.Alert_Error);
            }
        }


        private async Task ForwardExecute()
        {
            await DialogService.ShowAlertAsync(AppResource.Alert_FeatureNotLive, AppResource.Alert_Error);
        }

        private async Task NewMessageExecute()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await NavigationService.NavigateToPopupAsync<ComposeViewModel>(true);
            }
            catch (Exception ex)
            {
                if (ex is RpcClientUnknownException || ex is RpcClientTimeoutException) //todo switch error message
                {
                    AppSettings.ChangeRpcServer();
                }
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task MoreExecute()
        {
            await DialogService.ShowAlertAsync(AppResource.Alert_FeatureNotLive, AppResource.Alert_Error);
        }

        private async Task OpenTxExecute()
        {
            if (!string.IsNullOrEmpty(SelectedMessage.Hash))
            {
                var tx = SelectedMessage.Hash.Substring(2);
                var uri = new Uri(AppSettings.NeoScanTransactionsUrlEndPoint + tx);
                await Browser.OpenAsync(uri, BrowserLaunchType.SystemPreferred);
            }
        }

        private async Task ReplyExecute()
        {
            await NavigationService.NavigateToPopupAsync<ComposeViewModel>(SelectedMessage.FromInbox,true);
        }

        #region Observable Properties

        private Message _selectedMessage;

        public Message SelectedMessage
        {
            get => _selectedMessage;
            set
            {
                _selectedMessage = value;
                OnPropertyChanged();
            }
        }

        private string _daysAgo;

        public string DaysAgo
        {
            get => _daysAgo;
            set
            {
                _daysAgo = value;
                OnPropertyChanged();
            }
        }

        private bool _fromInbox;

        public bool FromInbox
        {
            get => _fromInbox;
            set
            {
                _fromInbox = value;
                OnPropertyChanged();
            }
        }

        private string _fromOrTo;

        public string FromOrTo
        {
            get => _fromOrTo;
            set
            {
                _fromOrTo = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}