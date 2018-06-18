using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Newtonsoft.Json;
using PhantasmaMail.Models;
using PhantasmaMail.Resources;
using PhantasmaMail.Services.Db;
using PhantasmaMail.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class ComposeViewModel : ViewModelBase
    {
        private readonly IPhantasmaDb _db;

        public ComposeViewModel(IPhantasmaDb phantasmaDb)
        {
            _db = phantasmaDb;
        }

        public ICommand CloseComposeCommand => new Command(async () => await CloseComposeExecute());
        public ICommand SendMessageCommand => new Command(async () => await SendMessageExecute());
        public ICommand AttachFileCommand => new Command(async () => await AttachFileExecute());
        public ICommand SaveDraftCommand => new Command(async () => await SaveDraftExecute());


        public override Task InitializeAsync(object navigationData)
        {
            Message = new Message();
            FormattedDate = $"{DateTime.Now:f}";

            if (navigationData is string s)
            {
                Message.ToInbox = s;
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task CloseComposeExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await NavigationService.PopAllAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SendMessageExecute()
        {
            if (IsBusy) return;
            if (string.IsNullOrEmpty(Message.Subject) || string.IsNullOrEmpty(Message.ToInbox) ||
                string.IsNullOrEmpty(Message.TextContent))
            {
                await DialogService.ShowAlertAsync("All fields are required", AppResource.Alert_Error);
                return;
            }

            var txHash = string.Empty;
            try
            {
                IsBusy = true;

                DialogService.ShowLoading();

                var toAddress = await PhantasmaService.GetAddressFromMailbox(Message.ToInbox.ToLowerInvariant());

                if (string.IsNullOrEmpty(toAddress))
                {
                    await DialogService.ShowAlertAsync("The specified inbox does not exist", AppResource.Alert_Error);
                    return;
                }

                Message.ToAddress = toAddress;
                var hashedMessage = SerializeAndHashMessage();
                txHash = await PhantasmaService.SendMessage(Message.ToInbox.ToLowerInvariant(), hashedMessage);
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }

            if (!string.IsNullOrEmpty(txHash))
            {
                Message.Hash = txHash;

                //store to db
                var store = Message.ToStoreMessage();
                if (store != null)
                {
                    await _db.AddMessage(store);
                }

                await DialogService.ShowAlertAsync(
                    "Message sent! Use a block explorer to see your transaction: " + txHash, "Success");
                await NavigationService.NavigateToAsync<InboxViewModel>();
            }
            else
            {
                await DialogService.ShowAlertAsync(AppResource.Alert_SomethingWrong, AppResource.Alert_Error);
            }

        }

        private async Task SaveDraftExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var draft = new DraftMessage
                {
                    Subject = Message.Subject,
                    TextContent = Message.TextContent,
                    ToInbox = Message.ToInbox
                };
                if (!await _db.AddMessage(draft)) // not saved
                {

                }

            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string SerializeAndHashMessage()
        {
            Message.Date = DateTime.UtcNow;
            Message.FromInbox = AuthenticationService.AuthenticatedUser.UserBox;
            Message.FromAddress = AuthenticationService.AuthenticatedUser.GetUserDefaultAddress();

            // todo
            //if (AppSettings.UseEncryption)
            //{
            //    var encryptedText = EncryptionUtils.Encrypt(Message.TextContent,
            //        AuthenticationService.AuthenticatedUser.GetPrivateKey(), new Byte[] { });
            //    Message.Key;
            //}

            var json = JsonConvert.SerializeObject(Message, AppSettings.JsonSettings());

            var bytes = Encoding.Default.GetBytes(json);
            json = Encoding.UTF8.GetString(bytes);

            return json;
        }

        private async Task AttachFileExecute()
        {
            await DialogService.ShowAlertAsync(AppResource.Alert_FeatureNotLive, AppResource.Alert_Error);
        }

        #region Observable Properties

        private Message _message;

        public Message Message
        {
            get => _message;

            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        //todo remove
        private string _formattedDate;

        public string FormattedDate
        {
            get => _formattedDate;
            set
            {
                _formattedDate = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}