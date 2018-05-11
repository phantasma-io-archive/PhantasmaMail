using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.Services;
using Newtonsoft.Json;
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

        public static JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static ObservableCollection<Message> SentMessages = new ObservableCollection<Message>();
    }
}
