using System;
using System.Threading.Tasks;
using System.Windows.Input;
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
            IsBusy = true;
            try
            {
                if (!string.IsNullOrEmpty(BoxName))
                {
                    var tx = await PhantasmaService.RegisterMailbox(BoxName);
                    if (string.IsNullOrEmpty(tx))
                    {
                        await DialogService.ShowAlertAsync("Something went wrong", "Error");
                    }
                    else
                    {
                        await DialogService.ShowAlertAsync(
                            "Box created, you need to wait 20/40 seconds minutes before sending any messages",
                            "Success");
                        await NavigationService.NavigateToAsync<MainViewModel>();
                    }
                }
            }
            catch (Exception)
            {
                await DialogService.ShowAlertAsync("Something went wrong. Make sure you have a drop of GAS in this address", "Error");
            }

            IsBusy = false;
        }
    }
}
