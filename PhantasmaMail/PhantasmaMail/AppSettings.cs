using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.Services;
using PhantasmaMail.Models;

namespace PhantasmaMail
{
    public static class AppSettings
    {
        //TODO change this to another spot
        public static INeoRestService RestService = new NeoScanRestService(NeoScanNet.TestNet);
        public static RpcClient RpcClient = new RpcClient(new Uri("http://seed5.neo.org:20332"));

		// Endpoints and other stuff
        // DEMO

        public static ObservableCollection<Message> SentMessages = new ObservableCollection<Message>
        {
            new Message()
            {
                TextContent = 
                    "It is a long established fact that a reader will be distracted by the readable content if you want any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for any desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for.",
                FromAddress = "test@phantasma.io",
                ToAddress =  DateTime.UtcNow.AddDays(-1).ToString("dd/MM/yyyy"),
                Subject = "This is a test"
            },
            new Message()
            {
                TextContent = "This is a small content",
                FromAddress = "test@phantasma.io",
                ToAddress = "John Test",
                Date = DateTime.UtcNow.AddDays(-1),
                Subject = "This is a test"
            },
        };
    }
}
