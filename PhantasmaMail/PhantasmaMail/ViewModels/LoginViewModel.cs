using PhantasmaMail.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Services.Phantasma;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
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

        private bool _isWif = false;

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


        public ICommand LoginCommand => new Command(async () => await LoginExecute());
        public ICommand SwitchLoginCommand => new Command(SwithLoginExecute);

        private async Task LoginExecute()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;

                // TODO LOGIN LOGIC
                if (IsWif)
                {
                    if (!await AuthenticationService.LoginAsync(Wif))
                    {
                        await DialogService.ShowAlertAsync("Invalid WIF", "Error", "Ok");
                        return;
                    }
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
                else
                {
                    if (!await AuthenticationService.LoginAsync(EncryptedKey, Password))
                    {
                        await DialogService.ShowAlertAsync("Invalid Encrypted Key/Password", "Error", "Ok");
                        return;
                    }
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }


        }

        public override Task InitializeAsync(object navigationData)
        {
            Wif = "L1mLVqjnuSHNeeGPpPq2aRv74Pm9TXJcXkhCJAz2K9s1Lrrd5fzH";
            return Task.FromResult(true);
        }

        private void SwithLoginExecute()
        {
            IsEncryptedKey = !IsEncryptedKey;
            IsWif = !IsWif;
        }
    }
}