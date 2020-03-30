using MessagingServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingServices.Application.Queue
{
	public interface IQueueRepository
	{
		bool PushMessage(UnifiedMessage message);
	}
}
