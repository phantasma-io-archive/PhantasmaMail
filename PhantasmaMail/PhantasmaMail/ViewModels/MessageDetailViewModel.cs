using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class MessageDetailViewModel : ViewModelBase
    {
        private Message _selectedMessage;

        public Message SelectedMessage
        {
            get => _selectedMessage;
            set
            {
                _selectedMessage = value;
                OnPropertyChanged();
            }
        }

        private string _daysAgo;

        public string DaysAgo
        {
            get => _daysAgo;
            set
            {
                _daysAgo = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteMessageCommand => new Command(async () => await DeleteMessageExecute());


        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Message message)
            {
                SelectedMessage = message;
                var d = CalculateDays(message.Date);
                DaysAgo = d + " days ago";
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task DeleteMessageExecute()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                //Execute delete method (local and remote)
                await NavigationService.NavigateBackAsync();
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

        //todo move this
        private int CalculateDays(DateTime date)
        {
            TimeSpan difference = DateTime.UtcNow - date;
            var days= Convert.ToInt32(difference.TotalDays);
            if (days == 0) return 1;
            return days;
        }
    }
}