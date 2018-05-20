using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Resources;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class RegisterBoxViewModel : ViewModelBase
    {
        private string _boxName;

        public string BoxName
        {
            get => _boxName;
            set
            {
                if (_boxName == value) return;
                _boxName = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateBoxCommand => new Command(async () => await CreateBoxExecute());

        private async Task CreateBoxExecute()
        {
            if (IsBusy) return;
            var tx = string.Empty;
            try
            {
                IsBusy = true;
                await Task.Delay(1000);
                if (!string.IsNullOrEmpty(BoxName))
                {
                    if (BoxName.Length <= 4 || BoxName.Length >= 20)
                    {
                        await DialogService.ShowAlertAsync(
                            "Box name length must be more than 4 characters and less than 20", AppResource.Alert_Error);
                        return;
                    }

                    tx = await PhantasmaService.RegisterMailbox(BoxName.ToLowerInvariant());
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(
                    "Something went wrong. Make sure you have at least one drop of GAS in this address", AppResource.Alert_Error);
            }
            finally
            {
                if (string.IsNullOrEmpty(tx))
                {
                    await DialogService.ShowAlertAsync(AppResource.Alert_SomethingWrong, AppResource.Alert_Error);
                }
                else
                {
                    AuthenticationService.AuthenticatedUser.UserBox = BoxName.ToLowerInvariant();
                    await DialogService.ShowAlertAsync(
                        "Box created, you need to wait 20/40 seconds minutes before sending any messages",
                        "Success");
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }

                IsBusy = false;
            }
        }
    }
}