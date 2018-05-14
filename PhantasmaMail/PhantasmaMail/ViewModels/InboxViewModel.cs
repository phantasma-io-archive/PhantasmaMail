using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class InboxViewModel : ViewModelBase
    {
        public InboxViewModel()
        {
            InboxList = new ObservableCollection<Message>();
        }

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());

        public ICommand MessageSelectedCommand =>
            new Command<Message>(async message => await MessageSelectedExecute(message));

        public ICommand RefreshCommand => new Command(async () => await RefreshExecute());

        public override async Task InitializeAsync(object navigationData)
        {
            DialogService.ShowLoading();
            await RefreshExecute();
            DialogService.HideLoading();
        }

        private async Task NewMessageExecute()
        {
            if (IsBusy) return;
            IsBusy = true;
            await NavigationService.NavigateToAsync<ComposeViewModel>();
            IsBusy = false;
        }

        private async Task MessageSelectedExecute(Message message)
        {
            if (message != null)
            {
                await NavigationService.NavigateToAsync<MessageDetailViewModel>(message);
                MessageSelected = null;
            }
        }

        public string BoxName { get; set; }

        public async Task RefreshExecute()
        {
            try
            {
                IsBusy = true;

                InboxList.Clear();
                //BoxName = await PhantasmaService.GetUserMailbox();
                //if (!string.IsNullOrEmpty(BoxName))
                //{
                //    var mailCount = await PhantasmaService.GetMailCount(BoxName);

                //    var emails = await PhantasmaService.GetMailsFromRange(BoxName, 1, mailCount);
                //    foreach (var email in emails)
                //    {
                //        if (email.StartsWith("{") || email.StartsWith("["))
                //        {
                //            var mailObject = JsonConvert.DeserializeObject<Message>(email, AppSettings.JsonSettings());
                //            if (mailObject != null) InboxList.Add(mailObject);
                //        }
                //    }

                //    InboxList = new ObservableCollection<Message>(InboxList.OrderByDescending(p => p.Date).ThenByDescending(p => p.Date.Hour).ToList());
                //}

                //var test = await PhantasmaService.MintTokens(50);
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error");
            }
            finally
            {
                IsBusy = false;
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