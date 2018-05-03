using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
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
            // todo REDO
            if (string.IsNullOrEmpty(Message.Subject) || string.IsNullOrEmpty(Message.ToAddress)) return;

            UserDialogs.Instance.ShowLoading();
            await Task.Delay(200);
            UserDialogs.Instance.HideLoading();
            await NavigationService.NavigateToAsync<InboxViewModel>();
            AppSettings.SentMessages.Insert(0, new InboxMessage
            {
                Subject = Message.Subject,
                Content = Message.TextContent,
                FromEmail = Message.FromAddress,
                FromName = "Relfos",
                ReceiveDate = DateTime.UtcNow.ToString("dd/MM/YYYY")
            });
        }

        private async Task AttachFileExecute()
        {
            await Task.Delay(1);
        }
    }
}