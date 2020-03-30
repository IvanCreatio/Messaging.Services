using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security;

namespace ThirdPartyProvider.MessengerVerifiers
{
	public class ThirdPartyMessageVerifierAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context) {
			string messengerId = context.ActionArguments["messengerId"] as string;
			string token = context.ActionArguments["token"] as string;
			ThirdPartyMessengerVerifier verifier = context.HttpContext.RequestServices.GetService<ThirdPartyMessengerVerifier>();
			if (token == null || messengerId == null || !verifier.Verify(token, messengerId)) {
				context.Result = new BadRequestResult();
				throw new SecurityException("ThirdParty verification error message");
			}
		}
	}
}
