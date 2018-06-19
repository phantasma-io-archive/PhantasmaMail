using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Services.Db;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class DraftsViewModel : ViewModelBase
    {
        private readonly IPhantasmaDb _db;

        public DraftsViewModel(IPhantasmaDb phantasmaDb)
        {
            _db = phantasmaDb;
        }

        public ICommand MessageSelectedCommand =>
            new Command<DraftMessage>(async message => await MessageSelectedExecute(message));

        public ICommand NewMessageCommand => new Command(async () => await NewMessageExecute());


        public override async Task InitializeAsync(object navigationData)
        {
            DialogService.ShowLoading();
            await RefreshList();
            DialogService.HideLoading();
        }

        public async Task RefreshList()
        {
            try
            {
                IsBusy = true;
                var drafts = await _db.GetDraftMessages();
                var draftMessages = drafts.OrderByDescending(msg => msg.Date).ToList();
                DraftsList = draftMessages.Any() ? new ObservableCollection<DraftMessage>(draftMessages) : null;
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

        private async Task MessageSelectedExecute(DraftMessage message)
        {
            if (message != null)
            {
                await NavigationService.NavigateToPopupAsync<ComposeViewModel>(message, true);
                MessageSelected = null;
            }
        }


        #region Observable Properties

        private ObservableCollection<DraftMessage> _draftsList;

        public ObservableCollection<DraftMessage> DraftsList
        {
            get => _draftsList;
            set
            {
                _draftsList = value;
                OnPropertyChanged();
            }
        }

        private DraftMessage _messageSelected;

        public DraftMessage MessageSelected
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