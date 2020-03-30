namespace FacebookProvider.MessengerVerifiers
{
	public interface IFacebookMessengerVerifier
	{
		bool VerifyToken(string facebookId, string facebookToken);

		bool VerifyFacebookId(string facebookId);
	}
}
