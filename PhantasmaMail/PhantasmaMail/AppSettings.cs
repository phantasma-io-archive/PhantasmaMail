using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using Newtonsoft.Json;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.Services.Phantasma;
using PhantasmaMail.ViewModels.Base;

namespace PhantasmaMail
{
    // Endpoints and other stuff
    public static class AppSettings
    {
        public static INeoRestService RestService = new NeoScanRestService(NeoScanNet.MainNet);

        //public static RpcClient RpcClient = new RpcClient(new Uri("http://seed2.aphelion-neo.com:10332"));
        public static RpcClient RpcClient = new RpcClient(new Uri("http://seed4.aphelion-neo.com:10332"));
        public static NeoNodesListService NodesService = new NeoNodesListService();
        public static string ContractScriptHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";

        //Transactions
        public static string NeoScanUrlTransactions = "https://neoscan.io/transaction/";

        public static TokenList TokenList { get; set; }

        public static bool UseEncryption { get; set; } = false; //todo

        public static bool UseMainNet { get; set; } = true;

        public static List<string> RpcUrlList => new List<string>
        {
            "http://seed1.cityofzion.io:8080",
            "http://seed2.cityofzion.io:8080",
            "http://seed3.cityofzion.io:8080",
            "http://seed4.cityofzion.io:8080",
            "http://seed5.cityofzion.io:8080",
            "http://api.otcgo.cn:10332",
            "https://seed1.neo.org:10331",
            "http://seed2.neo.org:10332",
            "http://seed3.neo.org:10332",
            "http://seed4.neo.org:10332",
            "http://seed5.neo.org:10332",
            "http://seed0.bridgeprotocol.io:10332",
            "http://seed1.bridgeprotocol.io:10332",
            "http://seed2.bridgeprotocol.io:10332",
            "http://seed3.bridgeprotocol.io:10332",
            "http://seed4.bridgeprotocol.io:10332",
            "http://seed5.bridgeprotocol.io:10332",
            "http://seed6.bridgeprotocol.io:10332",
            "http://seed7.bridgeprotocol.io:10332",
            "http://seed8.bridgeprotocol.io:10332",
            "http://seed9.bridgeprotocol.io:10332",
            "http://seed1.redpulse.com:10332",
            "http://seed2.redpulse.com:10332",
            "https://seed1.redpulse.com:10331",
            "https://seed2.redpulse.com:10331",
            "http://seed1.treatail.com:10332",
            "http://seed2.treatail.com:10332",
            "http://seed3.treatail.com:10332",
            "http://seed4.treatail.com:10332",
            "http://seed1.o3node.org:10332",
            "http://seed2.o3node.org:10332",
            "http://54.66.154.140:10332",
            "http://seed1.eu-central-1.fiatpeg.com:10332",
            "http://seed1.eu-west-2.fiatpeg.com:10332",
            "http://seed1.aphelion.org:10332",
            "http://seed2.aphelion.org:10332",
            "http://seed3.aphelion.org:10332",
            "http://seed4.aphelion.org:10332"
        };

        public static JsonSerializerSettings JsonSettings()
        {
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        private static readonly Random Rnd = new Random();

        public static async Task ChangeRpcServer()
        {
            //todo test net urls
            if (UseMainNet)
            {
                //var result = await NodesService.GetNodesList(MonitorNet.MainNet);
                //var nodes = JsonConvert.DeserializeObject<NodeList>(result);

                var index = Rnd.Next(RpcUrlList.Count);
                var rpcUrl = RpcUrlList[index];
                RpcClient = new RpcClient(new Uri(rpcUrl));
                Locator.Instance.Resolve<IPhantasmaService>().ApiService = new NeoApiService(RpcClient);
                Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser?.WalletManager.ChangeApiEndPoints(RpcClient, new NeoScanRestService(NeoScanNet.MainNet));
            }
            else
            {
                var index = Rnd.Next(RpcUrlList.Count);
                var rpcUrl = RpcUrlList[index];
                RpcClient = new RpcClient(new Uri(rpcUrl));
                Locator.Instance.Resolve<AuthenticationService>().AuthenticatedUser?.WalletManager.ChangeApiEndPoints(RpcClient, new NeoScanRestService(NeoScanNet.TestNet));
            }
        }
    }
}