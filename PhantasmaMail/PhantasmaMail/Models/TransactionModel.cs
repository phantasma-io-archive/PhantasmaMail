namespace PhantasmaMail.Models
{
    public class TransactionModel
    {
        public string TxHash { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Asset { get; set; }
        public decimal Amount { get; set; }
        public string Symbol { get; set; }
        public string ImagePath { get; set; }
    }
}