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

        public ICommand MessageSelectedCommand =>
            new Command<Message>(async message => await MessageSelectedExecute(message));

        public override async Task InitializeAsync(object navigationData)
        {
            InitTestList();
            await Task.Delay(1);
        }

        public SentViewModel()
        {
            //InitTestList();
        }


        private void InitTestList()
        {
            SentList = AppSettings.SentMessages;
        }

        private async Task MessageSelectedExecute(Message message)
        {
            await Task.Delay(1);
        }
    }
}