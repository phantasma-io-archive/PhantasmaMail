using PhantasmaMail.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            // TODO LOGIN LOGIC
            //await Task.Delay(1);
            await NavigationService.NavigateToAsync<MainViewModel>();
        }

        private void SwithLoginExecute()
        {
            IsEncryptedKey = !IsEncryptedKey;
            IsWif = !IsWif;
        }
    }
}