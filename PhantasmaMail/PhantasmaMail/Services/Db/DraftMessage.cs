using System;
using SQLite;

namespace PhantasmaMail.Services.Db
{
    [Table(nameof(DraftMessage))]
    public class DraftMessage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ToInbox { get; set; }

        public string Subject { get; set; }

        public string TextContent { get; set; }

        public DateTime Date { get; set; }
    }
}
