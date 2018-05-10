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

        public async Task RefreshExecute()
        {
            try
            {
                IsBusy = true;
                if (InboxList.Count > 0) return;
                InboxList.Clear();
                var name = await PhantasmaService.GetUserMailbox();
                if (!string.IsNullOrEmpty(name))
                {
                    var mailCount = await PhantasmaService.GetMailCount(name);

                    var emails = await PhantasmaService.GetMailsFromRange(name, 14, mailCount); //todo remove hardcoded 14
                    foreach (var email in emails)
                    {
                        var mailObject = JsonConvert.DeserializeObject<Message>(email);
                        InboxList.Add(mailObject);
                    }

                    InboxList =  new ObservableCollection<Message>(InboxList.OrderByDescending(p => p.Date).ThenByDescending(p => p.Date.Hour).ToList());
                }
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