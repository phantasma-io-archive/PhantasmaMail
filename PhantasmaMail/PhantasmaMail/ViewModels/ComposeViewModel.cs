using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Newtonsoft.Json;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class ComposeViewModel : ViewModelBase
    {
        private Message _message;

        public Message Message
        {
            get => _message;

            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public DateTime CurrentDateTime => DateTime.UtcNow;

        public ICommand NavigateToInboxCommand => new Command(async () => await NavigateToInboxExecute());
        public ICommand SendMessageCommand => new Command(async () => await SendMessageExecute());
        public ICommand AttachFileCommand => new Command(async () => await AttachFileExecute());

        public override Task InitializeAsync(object navigationData)
        {
            Message = new Message
            {
                ToAddress = "testNeoModules"
            };
            return base.InitializeAsync(navigationData);
        }


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
            if (IsBusy) return;
            if (string.IsNullOrEmpty(Message.Subject) || string.IsNullOrEmpty(Message.ToAddress) ||
                string.IsNullOrEmpty(Message.TextContent))
            {
                await DialogService.ShowAlertAsync("All fields are required", "Error");
                return;
            }

            var txHash = string.Empty;
            try
            {
                IsBusy = true;

                DialogService.ShowLoading();

                var hashedMessage = SerializeAndHashMessage();
                txHash = await PhantasmaService.SendMessage(Message.ToAddress, hashedMessage);
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error");
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }

            if (!string.IsNullOrEmpty(txHash))
            {
                await DialogService.ShowAlertAsync(
                    "Message sent! Use a block explorer to see your transaction: " + txHash, "Success");
                await NavigationService.NavigateToAsync<InboxViewModel>();
            }
            else
            {
                await DialogService.ShowAlertAsync("Something went wrong while sending the message", "Error");
            }
        }

        private string SerializeAndHashMessage()
        {
            Message.Date = DateTime.UtcNow;
            Message.FromAddress = AuthenticationService.AuthenticatedUser.GetUserDefaultAddress();
            var json = JsonConvert.SerializeObject(Message, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            });
            //TODO hash
            return json;
        }

        private async Task AttachFileExecute()
        {
            await Task.Delay(1);
        }
    }
}