using Xamarin.Forms;

namespace PhantasmaMail.Models
{
    public class SendMessage : BindableObject
    {
        #region Bindable Properties

        private string _toAddress;

        public string ToAddress
        {
            get => _toAddress;

            set
            {
                _toAddress = value;
                OnPropertyChanged();
            }
        }

        private string _fromAddress;

        public string FromAddress
        {
            get => _fromAddress;

            set
            {
                _fromAddress = value;
                OnPropertyChanged();
            }
        }

        private string _subject;

        public string Subject
        {
            get => _subject;

            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        private string _textContent;

        public string TextContent
        {
            get => _textContent;

            set
            {
                _textContent = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}