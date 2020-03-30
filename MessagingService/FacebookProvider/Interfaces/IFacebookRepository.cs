using FacebookProvider.Entities;

namespace FacebookProvider.Interfaces
{
	public interface IFacebookRepository
	{
		FacebookVerificationData GetMessengerVerificationData(string facebookId);
		string GetResponseUrl();
		string GetToken(string facebookId);
	}
}
