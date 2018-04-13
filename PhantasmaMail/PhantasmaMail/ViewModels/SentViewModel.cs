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
        // TODO: this list must be of SentMessage type
        private ObservableCollection<InboxMessage> _sentList;

        public ObservableCollection<InboxMessage> SentList
        {
            get => _sentList;
            set
            {
                _sentList = value;
                OnPropertyChanged();
            }
        }

        public ICommand MessageSelectedCommand =>
            new Command<InboxMessage>(async message => await MessageSelectedExecute(message));

        public override async Task InitializeAsync(object navigationData)
        {
            InitTestList();
            await Task.Delay(1);
        }


        private void InitTestList()
        {
            SentList = new ObservableCollection<InboxMessage>
            {
                new InboxMessage
                {
                    Content =
                        "It is a long established fact that a reader will be distracted by the readable content if you want any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for.",
                    FromEmail = "test@phantasma.io",
                    FromName = "John Test",
                    ReceiveDate = "string date",
                    Subject = "This is a test"
                },
                new InboxMessage
                {
                    Content = "This is a small content",
                    FromEmail = "test@phantasma.io",
                    FromName = "John Test",
                    ReceiveDate = "string date",
                    Subject = "This is a test"
                },
                new InboxMessage
                {
                    Content = "This is a long content dsadasdadsadsadadadsadadas dasdas asdas sa",
                    FromEmail = "test@phantasma.io",
                    FromName = "John Test",
                    ReceiveDate = "string date",
                    Subject = "This is a test"
                },
                new InboxMessage
                {
                    Content = "This is a long content dsadasdadsadsadadadsadadas dasdas asdas sa",
                    FromEmail = "test@phantasma.io",
                    FromName = "John Test",
                    ReceiveDate = "string date",
                    Subject = "This is a test"
                }
            };
        }

        private async Task MessageSelectedExecute(InboxMessage message)
        {
            await Task.Delay(1);
        }
    }
}