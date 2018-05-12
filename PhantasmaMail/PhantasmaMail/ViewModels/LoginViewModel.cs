using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using PhantasmaMail.Models;
using PhantasmaMail.Services.Db;
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
                DialogService.ShowLoading();
                IsBusy = true;
                // TODO LOGIN LOGIC
                if (IsWif)
                {
                    if (!await AuthenticationService.LoginAsync(Wif))
                    {
                        await DialogService.ShowAlertAsync("Invalid WIF", "Error");
                    }
                }
                else
                {
                    if (!await AuthenticationService.LoginAsync(EncryptedKey, Password))
                    {
                        await DialogService.ShowAlertAsync("Invalid Encrypted Key/Password", "Error");
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error");
            }
            finally
            {
                IsBusy = false;
                DialogService.HideLoading();
            }

            if (AuthenticationService.IsAuthenticated)
            {
                await LoadLocalFiles();
                var name = await PhantasmaService.GetUserMailbox();
                if (string.IsNullOrEmpty(name))
                {
                    await NavigationService.NavigateToAsync<RegisterBoxViewModel>();
                }
                else
                {
                    await NavigationService.NavigateToAsync<MainViewModel>();
                }
            }

        }

        private async Task LoadLocalFiles()
        {
            var rootfolder = await FileHelper.PhantasmaFolder.CreateFolder();
            if (rootfolder == null)
            {
                var folder = await FileHelper.PhantasmaFolder.CreateFolder();
                await FileHelper.DbFile.CreateFile(folder);
            }
            else
            {
                var json = await FileHelper.DbFile.ReadAllTextAsync(rootfolder);
                var list = JsonConvert.DeserializeObject<List<Message>>(json, AppSettings.JsonSettings());
                if (list != null)
                {
                    var sortedList = list.Where(p =>
                        p.FromAddress == AuthenticationService.AuthenticatedUser.GetUserDefaultAddress());
                    var enumerable = sortedList.ToList();
                    if (enumerable.Any())
                    {
                        AppSettings.SentMessages = new ObservableCollection<Message>(enumerable);
                    }
                    else
                    {
                        AppSettings.SentMessages = new ObservableCollection<Message>();
                    }
                }
               
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