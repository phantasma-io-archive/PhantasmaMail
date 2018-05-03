using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class InboxViewModel : ViewModelBase
    {
        private ObservableCollection<InboxMessage> _inboxList;

        public ObservableCollection<InboxMessage> InboxList
        {
            get => _inboxList;
            set
            {
                _inboxList = value;
                OnPropertyChanged();
            }
        }

        private InboxMessage _messageSelected;

        public InboxMessage MessageSelected
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

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());

        public ICommand MessageSelectedCommand =>
            new Command<InboxMessage>(async message => await MessageSelectedExecute(message));

        public ICommand RefreshCommand => new Command(async () => await RefreshExecute());

        public InboxViewModel()
        {
        }

        public override async Task InitializeAsync(object navigationData)
        {
            InitTestList();
            await Task.Delay(1);
        }


        private void InitTestList()
        {
            InboxList = new ObservableCollection<InboxMessage>
            {
                new InboxMessage
                {
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin quis fermentum quam, eget lacinia augue. In scelerisque, metus laoreet fringilla tincidunt, diam justo maximus dui, a auctor ex velit vitae tortor. Nam euismod malesuada libero, sed tempus magna. Duis at turpis in justo feugiat ornare sed eu metus. Praesent pharetra vulputate est, suscipit euismod odio faucibus quis. Curabitur ac ligula commodo, lacinia ante ut, porttitor odio. Nunc ultrices nisl vitae ligula posuere finibus. Suspendisse ultrices ex lorem, facilisis ullamcorper eros sollicitudin a. Vestibulum quis hendrerit odio.",
                    FromEmail = "relfos@phantasma.io",
                    FromName = "Relfos",
                    ReceiveDate = DateTime.UtcNow.ToString("dd/MM/yyyy"),
                    Subject = "Test message from Phantasma"
                },
                new InboxMessage
                {
                    Content = "Hello from Phantasma!",
                    FromEmail = "bruno@phantasma.io",
                    FromName = "Bruno",
                    ReceiveDate = DateTime.UtcNow.AddDays(-1).ToString("dd/MM/yyyy"),
                    Subject = "Hi"
                },
                new InboxMessage
                {
                    Content = "Duis laoreet eros libero, vitae accumsan sapien dignissim sit amet. Proin lobortis libero lorem, condimentum cursus risus porttitor quis. Nam in suscipit tortor. Aenean ullamcorper erat a lacus lacinia consequat.",
                    FromEmail = "miguel@phantasma.io",
                    FromName = "Miguel",
                    ReceiveDate = DateTime.UtcNow.AddDays(-4).ToString("dd/MM/yyyy"),
                    Subject = "Results from backend"
                },
                new InboxMessage
                {
                    Content = "Donec et tincidunt nulla. Fusce id vulputate lectus. Cras lacus ex, congue a velit rhoncus, fermentum condimentum neque.",
                    FromEmail = "alex@phantasma.io",
                    FromName = "Alex",
                    ReceiveDate = DateTime.UtcNow.AddDays(-10).ToString("dd/MM/yyyy"),
                    Subject = "Lorem ipsorum"
                }
            };
        }

        private async Task NewMessageExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await NavigationService.NavigateToAsync<DraftViewModel>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task MessageSelectedExecute(InboxMessage item)
        {
            if (item != null)
            {
                await NavigationService.NavigateToAsync<MessageDetailViewModel>(item);
                MessageSelected = null;
            }
        }

        private Task RefreshExecute()
        {
            //try
            //{
                throw new NotImplementedException();
            //}
            //catch (Exception ex)
            //{

            //}
        }
    }
}