using Newtonsoft.Json.Linq;
using PhantasmaMail.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhantasmaMail.Utils
{
    public static class CoinInfoUtils
    {
        public const int NEO_ID = 1376;
        public const int GAS_ID = 1785;

        private struct CacheInfo
        {
            public DateTime time;
            public string json;
        }

        private static Dictionary<int, CacheInfo> cache = new Dictionary<int, CacheInfo>();

        public static async Task<PriceInfo> GetMarketPrice(int coinID)
        {
            if (coinID <= 0)
            {
                return null;
            }

            string json;

            bool isCached = cache.ContainsKey(coinID) && (DateTime.UtcNow - cache[coinID].time).TotalMinutes<5;

            if (isCached)
            {
                json = cache[coinID].json;
            }
            else
            {
                var url = $"https://api.coinmarketcap.com/v2/ticker/{coinID}/";
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead);
                    json = await response.Content.ReadAsStringAsync();
                }

                var cacheEntry = new CacheInfo() { json = json, time = DateTime.UtcNow };
                cache[coinID] = cacheEntry;
            }

            var content = JObject.Parse(json)["data"];

            var info = new PriceInfo();
            info.name = content["name"].Value<string>();
            info.symbol = content["symbol"].Value<string>();
            info.rank = content["rank"].Value<int>();

            var quotes = content["quotes"]["USD"];
            info.price = quotes["price"].Value<decimal>();
            info.change = quotes["percent_change_24h"].Value<decimal>();

            return info;
        }

        internal static decimal CalculateChange(decimal balance, PriceInfo info)
        {
            decimal oldPrice = info.price + (info.price * (info.change/100m));
            return info.price - oldPrice;
        }

        // hardcoded until better solution
        public static int GetIDForSymbol(string symbol)
        {
            switch (symbol)
            {
                case "DBC": return 2316;
                case "RPX": return 2112;
                case "QLC": return 2321;
                case "ONT": return 2566;
                case "AVA": return 2776;
                case "SWTH": return 2620;
                case "EFX": return 2666;
                case "SOUL": return 2827;
                case "LRN": return 2693;
                case "TNC": return 2443;
                case "TKY": return 2507;
                case "APH": return 2684;
                case "NKN": return 2780;
                case "ACAT": return 2525;
                case "ZPT": return 2481;
                case "CPX": return 2641;
                default: return -1;
            }
        }
    }
}
