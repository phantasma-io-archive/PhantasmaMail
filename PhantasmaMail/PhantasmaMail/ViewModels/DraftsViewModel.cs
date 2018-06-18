using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class DraftsViewModel : ViewModelBase
    {

        public ICommand MessageSelectedCommand =>
            new Command<Message>(async message => await MessageSelectedExecute(message));





        private async Task MessageSelectedExecute(Message message) 
        {
            if (message != null)
            {
                await NavigationService.NavigateToAsync<MessageDetailViewModel>(new object[] { message, true }); //todo new type MessageDetailNavigationUtil
                MessageSelected = null;
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
