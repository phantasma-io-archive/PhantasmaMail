using NeoModules.Rest.DTOs.NeoNotifications;

namespace PhantasmaMail.Models
{
    public class AssetModel
    {
        public Token TokenDetails { get; set; }
        public decimal Amount { get; set; }
        public decimal FiatValue { get; set; }
        public decimal TotalFiatValue { get; set; }
        public decimal FiatChange { get; set; }
        public decimal FiatChangePercentage { get; set; }
        public string ImagePath { get; set; }
    }
}
