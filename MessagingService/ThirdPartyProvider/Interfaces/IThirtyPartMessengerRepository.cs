using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingServices.ThirdPartyProvider.Entities;

namespace ThirdPartyProvider.Api
{
	public interface IThirtyPartMessengerRepository
	{
		ThirtyPartMessenger GetMessenger(string messengerId);
	}
}
