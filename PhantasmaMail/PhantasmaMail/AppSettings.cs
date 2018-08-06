using System;
using System.Collections.Generic;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.DTOs.NeoNotifications;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using Newtonsoft.Json;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.Services.Phantasma;
using PhantasmaMail.ViewModels.Base;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PhantasmaMail
{
    // Endpoints and other stuff
    public static class AppSettings
    {

        // endpoints
        private const string DefaultRpcUrl = "http://seed4.aphelion-neo.com:10332";
        private const string DefaultNeoScanTransactionsUrl = "https://neoscan.io/transaction/";

        // services
        public static INeoscanService RestService = new NeoScanRestService(NeoScanNet.MainNet);
        public static NotificationsService NotificationsService = new NotificationsService();
        public static RpcClient RpcClient = new RpcClient(new Uri(DefaultRpcUrl));
        public static NeoNodesListService NodesService = new NeoNodesListService();

        // contract 
        public const string ContractScriptHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";


        public static TokenResult TokenList { get; set; }

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

        public static void ChangeRpcServer(string url = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                if (UseMainNet)
                {
                    //var result = await NodesService.GetNodesList(MonitorNet.MainNet);
                    //var nodes = JsonConvert.DeserializeObject<NodeList>(result);

                    var index = Rnd.Next(RpcUrlList.Count);
                    RpcUrlEndpoint = RpcUrlList[index];
                    RpcClient = new RpcClient(new Uri(RpcUrlEndpoint));
                    Locator.Instance.Resolve<IPhantasmaService>().ApiService = new NeoApiService(RpcClient);
                    Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser?.WalletManager.ChangeApiEndPoints(RpcClient, new NeoScanRestService(NeoScanNet.MainNet));
                }
                else
                {
                    var index = Rnd.Next(RpcUrlList.Count);
                    RpcUrlEndpoint = RpcUrlList[index];
                    RpcClient = new RpcClient(new Uri(RpcUrlEndpoint));
                    Locator.Instance.Resolve<AuthenticationService>().AuthenticatedUser?.WalletManager.ChangeApiEndPoints(RpcClient, new NeoScanRestService(NeoScanNet.TestNet));
                }
            }
            else
            {
                RpcUrlEndpoint = url;
                RpcClient = new RpcClient(new Uri(RpcUrlEndpoint));
                Locator.Instance.Resolve<IPhantasmaService>().ApiService = new NeoApiService(RpcClient);
                Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser?.WalletManager.ChangeApiEndPoints(RpcClient, new NeoScanRestService(NeoScanNet.MainNet));
            }
        }

        private static ISettings Settings => CrossSettings.Current;

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get => Settings.GetValueOrDefault(SettingsKey, SettingsDefault);
            set => Settings.AddOrUpdateValue(SettingsKey, value);
        }

        public static string NeoScanTransactionsUrlEndPoint
        {

            get => Settings.GetValueOrDefault(nameof(NeoScanTransactionsUrlEndPoint), DefaultNeoScanTransactionsUrl);

            set => Settings.AddOrUpdateValue(nameof(NeoScanTransactionsUrlEndPoint), value);
        }

        public static string RpcUrlEndpoint
        {

            get => Settings.GetValueOrDefault(nameof(RpcUrlEndpoint), DefaultRpcUrl);

            set => Settings.AddOrUpdateValue(nameof(RpcUrlEndpoint), value);
        }

    }
}