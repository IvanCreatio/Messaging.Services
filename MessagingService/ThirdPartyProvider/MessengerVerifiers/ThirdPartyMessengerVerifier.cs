using MessagingServices.ThirdPartyProvider.Entities;
using ThirdPartyProvider.Api;

namespace ThirdPartyProvider.MessengerVerifiers
{
	public class ThirdPartyMessengerVerifier
	{
		private readonly IThirtyPartMessengerRepository _messengerRepository;

		public ThirdPartyMessengerVerifier(IThirtyPartMessengerRepository repository) {
			_messengerRepository = repository;
		}

		public bool Verify(string token, string messengerId) {
			ThirtyPartMessenger messenger = _messengerRepository.GetMessenger(messengerId);
			return messenger.VerificationToken == token;
		}
	}
}
