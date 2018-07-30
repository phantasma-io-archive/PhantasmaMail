using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.JsonRpc.Client;
using PhantasmaMail.Resources;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public enum LoginEnum
    {
        Wif,
        EncryptedKey,
        Username
    }

    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand => new Command(async () => await LoginExecute());
        public ICommand SwitchLoginCommand => new Command<LoginEnum>(SwitchLoginExecute);


        public override Task InitializeAsync(object navigationData)
        {
            LoginOption = LoginEnum.EncryptedKey;
            return base.InitializeAsync(navigationData);
        }

        private async Task LoginExecute()
        {
            //if (isValid) TODO see if input is rigth
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                // TODO LOGIN LOGIC
                await Task.Delay(1000);
                switch (LoginOption)
                {
                    case LoginEnum.Wif:
                        if (!await AuthenticationService.LoginAsync(Wif))
                            await DialogService.ShowAlertAsync("Invalid WIF", AppResource.Alert_Error);
                        break;
                    case LoginEnum.EncryptedKey:
                        if (!await AuthenticationService.LoginAsync(EncryptedKey, Password))
                            await DialogService.ShowAlertAsync("Invalid Encrypted Key/Password", AppResource.Alert_Error);
                        break;
                    case LoginEnum.Username: //TODO
                        await DialogService.ShowAlertAsync("This is the least secure method to login. Use this option just to experiment the app, we do not advise to secure your funds here", "Info");
                        if (!await AuthenticationService.LoginWithUsername(Username, UsernamePassword))
                            await DialogService.ShowAlertAsync("Invalid Username/Password", AppResource.Alert_Error);
                        break;
                }
                await Task.Delay(500);

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

        private void SwitchLoginExecute(LoginEnum option)
        {
            LoginOption = option;
        }

        #region Observable Properties

        private LoginEnum _login;

        public LoginEnum LoginOption
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

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

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _usernamePassword;

        public string UsernamePassword
        {
            get => _usernamePassword;
            set
            {
                if (_usernamePassword == value) return;
                _usernamePassword = value;
                OnPropertyChanged();
            }
        }



        #endregion
    }
}