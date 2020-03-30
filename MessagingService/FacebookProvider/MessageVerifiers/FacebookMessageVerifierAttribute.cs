using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security;

namespace FacebookProvider.MessengerVerifiers
{
	public class FacebookMessageVerifierAttribute : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext context) {
			string facebookId = context.ActionArguments["facebookId"] as string;
			IFacebookMessengerVerifier verifier = context.HttpContext.RequestServices.GetService<IFacebookMessengerVerifier>();
			if (facebookId == null || !verifier.VerifyFacebookId(facebookId)) {
				context.Result = new BadRequestResult();
				throw new SecurityException("Facebook verification error message");
			}
		}
	}
}
