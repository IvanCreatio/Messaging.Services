using MessagingServices.Application.MessageConverters;
using MessagingServices.Domain.Entities;
using FacebookProvider.Entities;
using System;

namespace FacebookProvider.MessageConverters
{
	public class FacebookMessengerConverter : IMessageConverter<FacebookMessage>
	{
		public FacebookMessage ConvertToMessengerType(UnifiedMessage message) {
			throw new NotImplementedException();
		}

		public UnifiedMessage ConvertToUnifiedType(FacebookMessage message) {
			return new UnifiedMessage {
				Message = message.entry[0].messaging[0].message.text,  // <---------- fix array
				Recepient = message.entry[0].messaging[0].recipient.id,
				Sender = message.entry[0].messaging[0].sender.id,
				Timestapm = message.entry[0].messaging[0].timestamp
			};
		}
	}
}
