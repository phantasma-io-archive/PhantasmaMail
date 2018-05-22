using System;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.Services;
using Newtonsoft.Json;

namespace PhantasmaMail
{
    public static class AppSettings
    {  
        // Endpoints and other stuff
        //public static INeoRestService RestService = new NeoScanRestService(NeoScanNet.MainNet);
        //public static RpcClient RpcClient = new RpcClient(new Uri("http://seed1.cityofzion.io:8080"));

        public static INeoRestService RestService = new NeoScanRestService("http://89.115.152.211:4000/api/main_net/v1/");
        public static RpcClient RpcClient = new RpcClient(new Uri("http://89.115.152.211:30333"));

        //test ICO endpoints
        public static string NeoScanUrlTransactions = "https://neoscan.io/transaction/";

        public static JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            };
        }
    }
}
