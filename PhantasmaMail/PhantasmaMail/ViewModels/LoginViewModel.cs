using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.JsonRpc.Client;
using PhantasmaMail.Resources;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand => new Command(async () => await LoginExecute());
        public ICommand SwitchLoginCommand => new Command(SwithLoginExecute);

        private async Task LoginExecute()
        {
            //if (isValid) TODO see if input is rigth
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                // TODO LOGIN LOGIC
                await Task.Delay(1000);
                if (IsWif)
                {
                    if (!await AuthenticationService.LoginAsync(Wif))
                        await DialogService.ShowAlertAsync("Invalid WIF", AppResource.Alert_Error);
                }
                else
                {
                    if (!await AuthenticationService.LoginAsync(EncryptedKey, Password))
                        await DialogService.ShowAlertAsync("Invalid Encrypted Key/Password", AppResource.Alert_Error);
                }

                if (AuthenticationService.IsAuthenticated)
                {
                    var boxName = await PhantasmaService.GetMailboxFromAddress();
                    AuthenticationService.AuthenticatedUser.UserBox = boxName;

                    if (string.IsNullOrEmpty(boxName))
                    {
                        await NavigationService.NavigateToAsync<RegisterBoxViewModel>();
                    }
                    else
                    {
                        await NavigationService.NavigateToAsync<MainViewModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is RpcClientUnknownException || ex is RpcClientTimeoutException) //todo switch error message
                {
                    AppSettings.ChangeRpcServer();
                }
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SwithLoginExecute()
        {
            IsEncryptedKey = !IsEncryptedKey;
            IsWif = !IsWif;
        }

        #region Observable Properties

        private string _wif;

        public string Wif
        {
            get => _wif;
            set
            {
                if (_wif == value) return;
                _wif = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _encryptedKey;

        public string EncryptedKey
        {
            get => _encryptedKey;
            set
            {
                if (_encryptedKey == value) return;
                _encryptedKey = value;
                OnPropertyChanged();
            }
        }

        private bool _isEncryptedKey = true;

        public bool IsEncryptedKey
        {
            get => _isEncryptedKey;
            set
            {
                if (_isEncryptedKey == value) return;
                _isEncryptedKey = value;
                OnPropertyChanged();
            }
        }

        private bool _isWif;

        public bool IsWif
        {
            get => _isWif;
            set
            {
                if (_isWif == value) return;
                _isWif = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}