using MessagingServices.Application;
using MessagingServices.Application.Queue;
using MessagingServices.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MessagingServices.Api.Controllers
{
	[Route("api/message")]
	[ApiController]
	public class MessageController : Controller // Відкритий тільки для провайдерів ендпоінт
	{
		private readonly IQueueRepository _queueRepository;

		public MessageController(IQueueRepository queueRepository) {
			_queueRepository = queueRepository;
		}

		[HttpPost]
		[Route("receive/{messengerId}")]
		public void Receive(UnifiedMessage message, string messengerId) {
			_queueRepository.PushMessage(message);
		}
	}
}