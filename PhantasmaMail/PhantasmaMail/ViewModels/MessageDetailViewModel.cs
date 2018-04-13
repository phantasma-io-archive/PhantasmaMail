using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

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

        public ICommand DeleteMessageCommand => new Command(async () => await DeleteMessageExecute());


        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is InboxMessage message)
            {
                SelectedMessage = message;
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task DeleteMessageExecute()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                //Execute delete method (local and remote)
                await NavigationService.NavigateBackAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}