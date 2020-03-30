namespace MessagingServices.Domain.Entities
{
	public class UnifiedMessage
	{
		public string Sender { get; set; }
		public string Recepient { get; set; }
		public string Message { get; set; }
		public long Timestapm { get; set; }
		public MessengerType Source { get; set; }
	}
}
