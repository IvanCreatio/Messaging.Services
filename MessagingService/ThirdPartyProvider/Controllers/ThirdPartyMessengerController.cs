using MessagingServices.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using ThirdPartyProvider.MessengerVerifiers;

namespace MessagingServices.Api.Controllers
{
	[Route("api/[controller]")] //<---------- Роут для сторонніх мессенджерів
	[ApiController]
	public class ThirdPartyMessengerController : ControllerBase
	{


		private readonly IConfiguration _configuration;

		public ThirdPartyMessengerController(IConfiguration configuration) {
			_configuration = configuration;
		}

		private HttpWebRequest GetRequest(UnifiedMessage message, string messengerId) {
			var uri = new Uri("");  // <--- роут для зворотньої відправки
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.ContentType = "application/json";
			request.Method = "POST";
			using (var requestWriter = new StreamWriter(request.GetRequestStream())) {
				requestWriter.Write(JsonConvert.SerializeObject(message));
			}
			return request;
		}

		private void SendMessage(UnifiedMessage message, string messengerId) {
			try {
				HttpWebRequest request = GetRequest(message, messengerId);
				var response = (HttpWebResponse)request.GetResponse();
			} catch (Exception ex) {

			}
		}

		private async void SendToQueue(UnifiedMessage message) {
			var json = JsonConvert.SerializeObject(message);
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = _configuration.GetSection("MessagingServiceUrl").Value;
			using var client = new HttpClient();
			var response = await client.PostAsync(url, data);
			string result = response.Content.ReadAsStringAsync().Result;
		}

		[HttpPost]
		[Route("receive/{messengerId}")]
		[ThirdPartyMessageVerifier]
		public void Receive([FromBody]UnifiedMessage message, [FromBody]string token, string messengerId) { // параметри для валідації в атрибуті
			SendToQueue(message);
		}

		[HttpPost]
		[Route("send/{messengerId}")]
		[ThirdPartyMessageVerifier]
		public void Send([FromBody]UnifiedMessage message, [FromBody]string token, string messengerId) {
			SendMessage(message, messengerId);
		}

		[HttpPost]
		[Route("channel/{messengerId}")]
		[ThirdPartyMessageVerifier]
		public void ChannelSend([FromBody]UnifiedMessage message, [FromBody]string token, string messengerId) {
			SendMessage(message, messengerId);
		}
	}
}