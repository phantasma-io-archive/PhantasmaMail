using System;
using System.Collections.ObjectModel;
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

        //test ICO endpoints
        public static INeoRestService PrivateRestService = new NeoScanRestService("http://89.115.152.211:4000/api/main_net/v1/");
        public static RpcClient PrivateRpcClient = new RpcClient(new Uri("http://89.115.152.211:30333"));

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
