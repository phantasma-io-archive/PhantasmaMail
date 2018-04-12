using System.Threading.Tasks;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;

namespace PhantasmaMail.ViewModels
{
    public class MessageDetailViewModel : ViewModelBase
    {
        private InboxMessage _selectedMessage;

        public InboxMessage SelectedMessage
        {
            get => _selectedMessage;
            set
            {
                _selectedMessage = value;
                OnPropertyChanged();
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is InboxMessage message)
            {
                SelectedMessage = message;
            }
            return base.InitializeAsync(navigationData);
        }
    }
}
