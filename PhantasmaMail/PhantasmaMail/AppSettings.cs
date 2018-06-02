using System;
using System.Collections.Generic;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using Newtonsoft.Json;

namespace PhantasmaMail
{
    public static class AppSettings
    {
        // Endpoints and other stuff
        public static INeoRestService RestService = new NeoScanRestService(NeoScanNet.MainNet);
        public static RpcClient RpcClient = new RpcClient(new Uri("http://seed1.cityofzion.io:8080"));

        //public static INeoRestService RestService = new NeoScanRestService("http://89.115.152.211:4000/api/main_net/v1/");
        //public static RpcClient RpcClient = new RpcClient(new Uri("http://89.115.152.211:30333"));

        //Contract 
        //private
        //public static string ContractScriptHash = "9c8c855d1b10bc47265f6889e9c87d6895b5c9ff";

        public static string ContractScriptHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";

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
