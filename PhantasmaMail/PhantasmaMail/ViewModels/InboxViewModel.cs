using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using Newtonsoft.Json;
using PhantasmaMail.Models;
using PhantasmaMail.Resources;
using PhantasmaMail.Services.Db;
using PhantasmaMail.Utils;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class InboxViewModel : ViewModelBase
    {
        private readonly IPhantasmaDb _db;

        private List<Message> _fullInboxList;

        public InboxViewModel()
        {
            InboxList = new ObservableCollection<Message>();
        }

        public InboxViewModel(IPhantasmaDb phantasmaDb)
        {
            _db = phantasmaDb;
            InboxList = new ObservableCollection<Message>();
        }

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());

        public ICommand MessageSelectedCommand =>
            new Command<Message>(async message => await MessageSelectedExecute(message));

        public ICommand RefreshCommand => new Command(async () => await RefreshExecute());

        public ICommand DeleteMessageCommand => new Command<Message>(async msg => await DeleteMessageExecute(msg));

        public ICommand SearchCommand => new Command<string>(SearchExecute);

        public override async Task InitializeAsync(object navigationData)
        {
            DialogService.ShowLoading();
            await RefreshExecute();
            DialogService.HideLoading();
        }

        private async Task NewMessageExecute()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                await NavigationService.NavigateToPopupAsync<ComposeViewModel>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task MessageSelectedExecute(Message message)
        {
            if (message != null)
            {
                await NavigationService.NavigateToAsync<MessageDetailViewModel>(new object[] { message, true });
                MessageSelected = null;
            }
        }

        public async Task RefreshExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                InboxList = new ObservableCollection<Message>();
                if (!string.IsNullOrEmpty(AuthenticationService.AuthenticatedUser.UserBox))
                {
                    var publicKey = AuthenticationService.AuthenticatedUser.GetPublicKey();
                    var mailCount = await PhantasmaService.GetInboxCount();
                    if (mailCount > 0)
                    {
                        await DeserializeInboxMails(mailCount);
                    }
                }
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

        private async Task DeserializeInboxMails(int mailCount)
        {
            var index = 1;
            var emails = await PhantasmaService.GetAllInboxMessages(mailCount);
            try
            {
                //deserialization
                foreach (var email in emails)
                {
                    if (email.StartsWith("{") || email.StartsWith("["))
                    {
                        var mailObject =
                            JsonConvert.DeserializeObject<Message>(email, AppSettings.JsonSettings());
                        if (MessageUtils.IsHex(mailObject.TextContent.ToCharArray()))
                        {
                            var encryptedText = mailObject.TextContent.HexToBytes();
                            var remotePub = await PhantasmaService.GetMailboxPublicKey(mailObject.FromInbox);
                            var decryptedText = EncryptionUtils.Decrypt(encryptedText,
                                AuthenticationService.AuthenticatedUser.GetPrivateKey(), remotePub.HexToBytes());
                            mailObject.TextContent = decryptedText;
                        }
                        mailObject.ID = index;
                        InboxList.Add(mailObject);
                        index++;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            InboxList = new ObservableCollection<Message>(InboxList.OrderByDescending(p => p.Date)
                .ThenByDescending(p => p.Date.Hour).ToList());
            _fullInboxList = InboxList.ToList();
        }

        private async Task DeleteMessageExecute(Message msg)
        {
            if (msg == null) return;
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var tx = await PhantasmaService.RemoveInboxMessage(msg.ID);
                if (string.IsNullOrEmpty(tx))
                    await DialogService.ShowAlertAsync(AppResource.Alert_SomethingWrong, AppResource.Alert_Error);
                else
                {
                    _fullInboxList.Remove(msg);
                    InboxList = new ObservableCollection<Message>(_fullInboxList);
                }

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

        private void SearchExecute(string text)
        {
            if (InboxList.Count == 0) return;
            if (string.IsNullOrEmpty(text))
            {
                InboxList = new ObservableCollection<Message>(_fullInboxList);
            }
            else
            {
                var newList = new List<Message>(_fullInboxList.Where(msg => msg.TextContent != null
                                                                            && (msg.TextContent.Contains(text)
                                                                            || msg.ToInbox.Contains(text)
                                                                            || msg.Subject.Contains(text)
                                                                            || msg.FromInbox.Contains(text))));
                InboxList = new ObservableCollection<Message>(newList);
            }
        }

        #region Observable Properties

        private ObservableCollection<Message> _inboxList;

        public ObservableCollection<Message> InboxList
        {
            get => _inboxList;
            set
            {
                _inboxList = value;
                OnPropertyChanged();
            }
        }

        private Message _messageSelected;

        public Message MessageSelected
        {
            get => _messageSelected;
            set
            {
                if (_messageSelected != value)
                {
                    _messageSelected = value;
                    OnPropertyChanged();
                    MessageSelectedCommand.Execute(_messageSelected);
                }
            }
        }

        #endregion
    }
}