using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace PhantasmaMail.Models
{
    public class Message : BindableObject
    {
        #region Bindable Properties

        private string _toAddress;

        [JsonProperty("to")]
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

        [JsonProperty("from")]
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

        [JsonProperty("subject")]
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

        [JsonProperty("content")]
        public string TextContent
        {
            get => _textContent;

            set
            {
                _textContent = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;

        [JsonProperty("date")]
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        private string _hash;

        [JsonIgnore]
        public string Hash
        {
            get => _hash;
            set
            {
                _hash = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public static async Task Store()
        {
            //if (this.hash != null)
            //{
            //    return this.hash;
            //}

            //var ipfs = new IpfsClient();

            //var text = JsonSerializer.WriteToString(this.TextContent);
            //var node = await ipfs.FileSystem.AddTextAsync(text);
            //this.hash = node.Hash;

            //return this.hash;
            await Task.Delay(1);
        }
    }
}