using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PhantasmaMail.Models;

namespace PhantasmaMail
{
    public static class AppSettings
    {
		//Endpoints and other stuff
        // DEMO

        public static ObservableCollection<InboxMessage> SentMessages = new ObservableCollection<InboxMessage>
        {
            new InboxMessage
            {
                Content =
                    "It is a long established fact that a reader will be distracted by the readable content if you want any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for.",
                FromEmail = "test@phantasma.io",
                FromName = "John Test",
                ReceiveDate =  DateTime.UtcNow.AddDays(-1).ToString("dd/MM/yyyy"),
                Subject = "This is a test"
            },
            new InboxMessage
            {
                Content = "This is a small content",
                FromEmail = "test@phantasma.io",
                FromName = "John Test",
                ReceiveDate = DateTime.UtcNow.AddDays(-1).ToString("dd/MM/yyyy"),
                Subject = "This is a test"
            },
        };
    }
}
