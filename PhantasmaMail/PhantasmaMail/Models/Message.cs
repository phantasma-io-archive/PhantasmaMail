using System;
using Newtonsoft.Json;
using PhantasmaMail.Services.Db;
using Xamarin.Forms;

namespace PhantasmaMail.Models
{
    public class Message : BindableObject
    {
        [JsonIgnore]
        public int ID { get; set; }

        #region Bindable Properties

        private string _toInbox;

        [JsonProperty("toInbox")]
        public string ToInbox
        {
            get => _toInbox;

            set
            {
                _toInbox = value;
                OnPropertyChanged();
            }
        }

        private string _toAddress;

        [JsonProperty("toAddress")]
        public string ToAddress
        {
            get => _toAddress;

            set
            {
                _toAddress = value;
                OnPropertyChanged();
            }
        }


        private string _fromInbox;

        [JsonProperty("fromInbox")]
        public string FromInbox
        {
            get => _fromInbox;

            set
            {
                _fromInbox = value;
                OnPropertyChanged();
            }
        }


        private string _fromAddress;

        [JsonProperty("fromAddress")]
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

        [JsonProperty("txid")]
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


        public Message(StoreMessage storeMessage)
        {
            FromAddress = storeMessage.FromAddress;
            FromInbox = storeMessage.FromInbox;
            ToAddress = storeMessage.ToAddress;
            ToInbox = storeMessage.ToInbox;
            Date = storeMessage.Date;
            Hash = storeMessage.Hash;
            Subject = storeMessage.Subject;
            TextContent = storeMessage.TextContent;
        }

        public Message()
        {

        }

        public StoreMessage ToStoreMessage()
        {
            var dbMessage = new StoreMessage
            {
                Date = Date,
                FromInbox = FromInbox,
                FromAddress = FromAddress,
                ToAddress = ToAddress,
                ToInbox = ToInbox,
                Hash = Hash,
                TextContent = TextContent,
                Subject = Subject,
                ID = ID
            };
            return dbMessage;
        }
    }
}