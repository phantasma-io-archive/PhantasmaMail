namespace PhantasmaMail.Models
{
    public class TransactionModel
    {
        public string TxHash { get; set; }
        public string Type { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public decimal Amount { get; set; }
        public string Symbol { get; set; }
    }
}