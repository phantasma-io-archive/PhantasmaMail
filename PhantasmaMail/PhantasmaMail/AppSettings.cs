using System;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.Services;
using Newtonsoft.Json;

namespace PhantasmaMail
{
    public static class AppSettings
    {  
        // Endpoints and other stuff
        public static INeoRestService RestService = new NeoScanRestService(NeoScanNet.MainNet);
        public static RpcClient RpcClient = new RpcClient(new Uri("http://seed1.cityofzion.io:8080"));

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
