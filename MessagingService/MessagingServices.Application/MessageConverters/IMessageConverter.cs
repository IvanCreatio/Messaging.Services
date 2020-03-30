using MessagingServices.Domain.Entities;

namespace MessagingServices.Application.MessageConverters
{
	public interface IMessageConverter<T>
	{

		UnifiedMessage ConvertToUnifiedType(T message);

		T ConvertToMessengerType(UnifiedMessage message);
	}
}
