using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class DraftViewModel : ViewModelBase // todo change draft to "compose" wtv
    {
        private SendMessage _message;
        public SendMessage Message
        {
            get => _message;

            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public DraftViewModel()
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            Message = new SendMessage();
            return base.InitializeAsync(navigationData);
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
            // todo check Message content
            if (string.IsNullOrEmpty(Message.Subject) || string.IsNullOrEmpty(Message.ToAddress)) return;
            await Task.Delay(1);
        }

        private async Task AttachFileExecute()
        {
            await Task.Delay(1);
        }
    }
}