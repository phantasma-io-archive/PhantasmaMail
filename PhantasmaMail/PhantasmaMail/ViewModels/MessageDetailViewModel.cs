using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using PhantasmaMail.Models;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class MessageDetailViewModel : ViewModelBase
    {
        public ICommand DeleteMessageCommand => new Command(async () => await DeleteMessageExecute());


        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is object[] data && data[0] is Message message && data[1] is bool fromInbox)
            {
                FromInbox = fromInbox;
                if (FromInbox) //comes from Inbox
                {
                    FromOrTo = "From: "; //todo localization
                    SelectedMessage = message;
                    var culture = new CultureInfo("en-GB");
                    CultureInfo.CurrentCulture = culture;
                    DaysAgo = CalculateDays(message.Date);
                    FormattedDate = string.Format("{0:f}", message.Date);
                }
                else //comes from Sent
                {
                    FromOrTo = "To: "; //todo localization
                    SelectedMessage = message;
                    var culture = new CultureInfo("en-GB");
                    CultureInfo.CurrentCulture = culture;
                    DaysAgo = CalculateDays(message.Date);
                    FormattedDate = string.Format("{0:f}", message.Date);
                }
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
        private string CalculateDays(DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            var s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            var dayDiff = (int) s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            var secDiff = (int) s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31) return null;

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60) return "just now";
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120) return "1 minute ago";
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                    return string.Format("{0} minutes ago",
                        Math.Floor((double) secDiff / 60));
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200) return "1 hour ago";
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                    return string.Format("{0} hours ago",
                        Math.Floor((double) secDiff / 3600));
            }

            // 6.
            // Handle previous days.
            if (dayDiff == 1) return "yesterday";
            if (dayDiff < 7)
                return string.Format("{0} days ago",
                    dayDiff);
            if (dayDiff < 31)
                return string.Format("{0} weeks ago",
                    Math.Ceiling((double) dayDiff / 7));
            return null;
        }

        #region Observable Properties

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

        private string _formattedDate;

        public string FormattedDate
        {
            get => _formattedDate;
            set
            {
                _formattedDate = value;
                OnPropertyChanged();
            }
        }

        private bool _fromInbox;

        public bool FromInbox
        {
            get => _fromInbox;
            set
            {
                _fromInbox = value;
                OnPropertyChanged();
            }
        }

        private string _fromOrTo;

        public string FromOrTo
        {
            get => _fromOrTo;
            set
            {
                _fromOrTo = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}