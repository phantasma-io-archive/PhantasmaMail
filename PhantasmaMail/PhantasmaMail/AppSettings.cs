using System;
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
        public static RpcClient RpcClient = new RpcClient(new Uri("http://seed2.neo.org:10332"));
        public static string ContractScriptHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";

        public static TokenList TokenList { get; set; }

        //Transactions
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
