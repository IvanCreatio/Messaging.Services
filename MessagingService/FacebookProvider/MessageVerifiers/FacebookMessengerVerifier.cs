using FacebookProvider.Entities;
using FacebookProvider.Interfaces;

namespace FacebookProvider.MessengerVerifiers
{
	public class FacebookMessengerVerifier : IFacebookMessengerVerifier
	{
		private readonly IFacebookRepository _facebookRepository;

		public FacebookMessengerVerifier(IFacebookRepository repository) {
			_facebookRepository = repository;
		}

		private bool VerifyFacebookId(string facebookId, FacebookVerificationData data) {
			return facebookId == data.FacebookIdentifier;
		}

		public bool VerifyToken(string facebookId, string facebookToken) {
			var verificationData = _facebookRepository.GetMessengerVerificationData(facebookId);
			return verificationData.Token == facebookToken && VerifyFacebookId(facebookId, verificationData);
		}

		public bool VerifyFacebookId(string facebookId) {
			var verificationData = _facebookRepository.GetMessengerVerificationData(facebookId);
			return VerifyFacebookId(facebookId, verificationData);
		}
	}
}
