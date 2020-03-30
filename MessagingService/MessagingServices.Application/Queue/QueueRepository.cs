using MessagingServices.Domain.Entities;
using System.Collections.Generic;

namespace MessagingServices.Application.Queue
{
	public class QueueRepository : IQueueRepository
	{
		private static Queue<UnifiedMessage> unifiedMessagesQueue = new Queue<UnifiedMessage>();

		public bool PushMessage(UnifiedMessage message) {
			bool isQueueRunning = true;// <---- need to check status of queue
			unifiedMessagesQueue.Enqueue(message);
			return isQueueRunning; 
		}
	}
}
