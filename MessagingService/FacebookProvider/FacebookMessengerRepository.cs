using FacebookProvider.Entities;
using FacebookProvider.Interfaces;

namespace FacebookProvider
{
	public class FacebookMessengerRepository : IFacebookRepository
	{

		public FacebookVerificationData GetMessengerVerificationData(string facebookId) {
			return new FacebookVerificationData();
		}

		public string GetResponseUrl() {
			return "https://graph.facebook.com/v2.6/me/messages?access_token=";
		}

		public string GetToken(string facebookId) {
			return new FacebookVerificationData().Token;
		}
	}
}
