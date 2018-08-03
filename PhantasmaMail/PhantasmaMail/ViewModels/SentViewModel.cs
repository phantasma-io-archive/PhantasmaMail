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
    public class SentViewModel : ViewModelBase
    {
        private readonly IPhantasmaDb _db;

        private List<Message> _fullSentList;

        public SentViewModel(IPhantasmaDb phantasmaDb)
        {
            _db = phantasmaDb;
            SentList = new ObservableCollection<Message>();
        }

        public ICommand MessageSelectedCommand =>
            new Command<Message>(async message => await MessageSelectedExecute(message));

        public ICommand RefreshCommand => new Command(async () => await RefreshExecute());

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());

        public ICommand SearchCommand => new Command<string>(SearchExecute);

        public ICommand ActivateMultipleSelectionCommand => new Command(ActivateMultipleSelectionExecute);

        public ICommand DeleteSelectedMessages => new Command(async () => await DeleteSelectedMessagesExecute());

        public override async Task InitializeAsync(object navigationData)
        {
            DialogService.ShowLoading();
            await RefreshExecute();
            DialogService.HideLoading();
        }

        private void ActivateMultipleSelectionExecute()
        {
            IsMultipleSelectionActive = !IsMultipleSelectionActive;
            if (IsMultipleSelectionActive) return;
            DeselectAllMessages();
        }

        private void DeselectAllMessages()
        {
            foreach (var message in SentList) message.IsSelected = false;
        }

        private async Task NewMessageExecute()
        {
            if (IsBusy) return;
            IsBusy = true;
            await NavigationService.NavigateToPopupAsync<ComposeViewModel>(true);
            IsBusy = false;
        }

        public async Task RefreshExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                SentList = new ObservableCollection<Message>();
                if (!string.IsNullOrEmpty(AuthenticationService.AuthenticatedUser.UserBox))
                {
                    var mailCount = await PhantasmaService.GetOutboxCount();
                    if (mailCount > 0)
                    {
                        await DeserializeOutboxMails(mailCount);
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

        private async Task DeserializeOutboxMails(int mailCount)
        {
            var index = 1;
            var emails = await PhantasmaService.GetAllOutboxMessages(mailCount);
            var storedEmails = await _db.GetSentMessages(AuthenticationService.AuthenticatedUser.UserBox);
            try
            {
                //deserialization
                foreach (var email in emails)
                {
                    if (email.StartsWith("{") || email.StartsWith("["))
                    {
                        string decryptedText = string.Empty;
                        var mailObject =
                            JsonConvert.DeserializeObject<Message>(email, AppSettings.JsonSettings());
                        if (mailObject?.TextContent != null)
                        {
                            if (MessageUtils.IsHex(mailObject.TextContent.ToCharArray()))
                            {
                                var encryptedText = mailObject.TextContent.HexToBytes();
                                var remotePub = await PhantasmaService.GetMailboxPublicKey(mailObject.ToInbox);
                                decryptedText = EncryptionUtils.Decrypt(encryptedText,
                                    AuthenticationService.AuthenticatedUser.GetPrivateKey(), remotePub.HexToBytes());
                            }

                            mailObject.ID = index;
                            var hash = GetHashFromStoredMessage(storedEmails.ToList(), mailObject);
                            if (!string.IsNullOrEmpty(hash)) mailObject.Hash = hash;
                            if (!string.IsNullOrEmpty(decryptedText))
                            {
                                mailObject.TextContent = decryptedText;
                            }
                            SentList.Add(mailObject);
                            index++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            SentList = new ObservableCollection<Message>(SentList.OrderByDescending(p => p.Date)
                .ThenByDescending(p => p.Date.Hour).ToList());
            _fullSentList = SentList.ToList();
        }

        private async Task MessageSelectedExecute(Message message)
        {
            if (IsBusy) return;
            IsBusy = true;

            if (message != null)
            {
                if (IsMultipleSelectionActive)
                    message.IsSelected = !message.IsSelected;
                else
                    await NavigationService.NavigateToAsync<MessageDetailViewModel>(new object[] { message, false });
                MessageSelected = null;
            }

            IsBusy = false;
        }

        private async Task DeleteSelectedMessagesExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var indexes = new List<int>();
                foreach (var message in SentList)
                {
                    if (message.IsSelected)
                    {
                        indexes.Add(message.ID);
                    }
                }
                var tx = await PhantasmaService.RemoveInboxMessages(indexes.ToArray());
                if (string.IsNullOrEmpty(tx))
                {
                    await DialogService.ShowAlertAsync(AppResource.Alert_SomethingWrong, AppResource.Alert_Error);
                }
                else
                {
                    _fullSentList.RemoveAll(msg => indexes.Contains(msg.ID));
                    SentList = new ObservableCollection<Message>(_fullSentList);
                }
            }
            catch (Exception ex)
            {
                if (ex is RpcClientUnknownException || ex is RpcClientTimeoutException) //todo switch error message
                    AppSettings.ChangeRpcServer();
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                IsBusy = false;
                IsMultipleSelectionActive = false;
                DeselectAllMessages();
            }
        }

        private string GetHashFromStoredMessage(List<StoreMessage> storedMessages, Message msg)
        {
            //todo equals method
            var messageWithHash = storedMessages.Find(m => m.TextContent == msg.TextContent
                                                           && m.ToInbox == msg.ToInbox
                                                           && m.Date == msg.Date
                                                           && m.FromInbox == msg.FromInbox);

            return messageWithHash?.Hash;
        }

        private void SearchExecute(string text)
        {
            if (SentList.Count == 0) return;
            if (string.IsNullOrEmpty(text))
            {
                SentList = new ObservableCollection<Message>(_fullSentList);
            }
            else
            {
                var newList = new List<Message>(_fullSentList.Where(msg => msg.TextContent != null
                                                                           && (msg.TextContent.Contains(text)
                                                                           || msg.ToInbox.Contains(text)
                                                                           || msg.Subject.Contains(text)
                                                                           || msg.FromInbox.Contains(text))));
                SentList = new ObservableCollection<Message>(newList);
            }
        }

        #region Observable Properties

        private ObservableCollection<Message> _sentList;

        public ObservableCollection<Message> SentList
        {
            get => _sentList;
            set
            {
                _sentList = value;
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

        private bool _isMultipleSelectionActive;

        public bool IsMultipleSelectionActive
        {
            get => _isMultipleSelectionActive;
            set
            {
                _isMultipleSelectionActive = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}