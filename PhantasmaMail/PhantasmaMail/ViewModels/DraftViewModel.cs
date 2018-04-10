using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class DraftViewModel : ViewModelBase
    {
        public DraftViewModel()
        {
        }

        public ICommand NavigateToInboxCommand => new Command(async () => await NavigateToInboxExecute());
        public ICommand SendMessageCommand => new Command(async () => await SendMessageExecute());
        public ICommand AttachFileCommand => new Command(async () => await AttachFileExecute());


        private async Task NavigateToInboxExecute()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await NavigationService.NavigateToAsync<InboxViewModel>();
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

        private async Task SendMessageExecute()
        {
            await Task.Delay(1);
        }

        private async Task AttachFileExecute()
        {
            await Task.Delay(1);
        }
    }
}