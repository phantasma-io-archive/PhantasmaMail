using System;
using SQLite;

namespace PhantasmaMail.Services.Db
{
    [Table(nameof(StoreMessage))]
    public class StoreMessage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ToAddress { get; set; }

        public string ToInbox { get; set; }

        public string FromInbox { get; set; }

        public string FromAddress { get; set; }
   
        public string Subject { get; set; }

        public string TextContent { get; set; }

        public DateTime Date { get; set; }

        public string Hash { get; set; }
    }
}
