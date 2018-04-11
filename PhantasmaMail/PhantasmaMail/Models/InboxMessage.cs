namespace PhantasmaMail.Models
{
	//TODO
    public class InboxMessage
    {
		public string Subject { get; set; } = string.Empty;
		public string Content { get; set; } = string.Empty;
		public string FromName { get; set; } = string.Empty;
		public string FromEmail { get; set; } = string.Empty;
		public string ReceiveDate { get; set; } = string.Empty;
	}
}
