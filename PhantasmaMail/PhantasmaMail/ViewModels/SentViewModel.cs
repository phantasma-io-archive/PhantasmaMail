using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class SentViewModel : ViewModelBase
    {
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
        #endregion


        public ICommand MessageSelectedCommand =>
            new Command<Message>(async message => await MessageSelectedExecute(message));

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());

        public override async Task InitializeAsync(object navigationData)
        {
            InitTestList();
            await Task.Delay(1);
        }

        private async Task NewMessageExecute()
        {
            if (IsBusy) return;
            IsBusy = true;
            await NavigationService.NavigateToAsync<ComposeViewModel>();
            IsBusy = false;
        }

        private void InitTestList()
        {
            SentList = AppSettings.SentMessages;
        }

        private async Task MessageSelectedExecute(Message message)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (message != null)
            {
                await NavigationService.NavigateToAsync<MessageDetailViewModel>(message);
                MessageSelected = null;
            }
            await NavigationService.NavigateToAsync<MessageDetailViewModel>(message);
            IsBusy = false;
        }
    }
}