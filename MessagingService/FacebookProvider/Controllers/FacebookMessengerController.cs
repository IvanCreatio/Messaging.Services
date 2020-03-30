using FacebookProvider.Entities;
using FacebookProvider.Interfaces;
using FacebookProvider.MessengerVerifiers;
using MessagingServices.Application.MessageConverters;
using MessagingServices.Application.Queue;
using MessagingServices.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace MessagingServices.Api.Controllers
{
	[Route("api/providers/facebook/v1")]
	[ApiController]
	public class FacebookMessengerController : ControllerBase
	{

		private readonly IFacebookRepository _facebookRepository;
		private readonly IMessageConverter<FacebookMessage> _messageConverter;
		private readonly IQueueRepository _queueRepository;
		private readonly IConfiguration _configuration;
		private readonly IFacebookMessengerVerifier _verifier;

		public FacebookMessengerController(IFacebookRepository repository,
				IMessageConverter<FacebookMessage> converter,
				IFacebookMessengerVerifier verifier,
				IConfiguration configuration) {
			_facebookRepository = repository;
			_messageConverter = converter;
			_verifier = verifier;
			_configuration = configuration;
		}

		private HttpWebRequest GetRequestToFacebook(FacebookMessage facebookMessage, string facebookToken) {
			// Переписати на щось краще за HttpWebRequest
			string facebookResponseUrl = _facebookRepository.GetResponseUrl();
			var content = facebookMessage.entry[0].messaging[0];
			var uri = new Uri(string.Concat(facebookResponseUrl, facebookToken));
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.ContentType = "application/json";
			request.Method = "POST";
			using (var requestWriter = new StreamWriter(request.GetRequestStream())) {
				requestWriter.Write($@" {{recipient: {{  id: {content.sender.id}}},message: {{text: ""{content.message.text}"" }}}}");
			}
			return request;
		}

		private void SendMessage(UnifiedMessage message, string facebookId) {
			try {
				FacebookMessage facebookMessage = _messageConverter.ConvertToMessengerType(message);
				string facebookToken = _facebookRepository.GetToken(facebookId);
				HttpWebRequest request = GetRequestToFacebook(facebookMessage, facebookToken);
				var response = (HttpWebResponse)request.GetResponse();
			} catch (Exception ex) {

			}
		}

		private async void SendToQueue(UnifiedMessage message) {
			var json = JsonConvert.SerializeObject(message);
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = _configuration.GetSection("MessagingServiceUrl").Value; // https://messaging.creatio.com/api/message
			using var client = new HttpClient();
			var response = await client.PostAsync(url, data);
			string result = response.Content.ReadAsStringAsync().Result;
		}

		[HttpGet]
		[Route("receive/{facebookId}")]
		public string VerifyToken(string facebookId) {
			string token = Request.Query["hub.verify_token"].FirstOrDefault();
			return _verifier.VerifyToken(facebookId, token) ? token : string.Empty;
		}

		[HttpPost]
		[Route("receive/{facebookId}")]
		[FacebookMessageVerifier]
		public void Receive([FromBody]FacebookMessage message, string facebookId) {
			var unifiedMessage = _messageConverter.ConvertToUnifiedType(message);
			unifiedMessage.Source = MessengerType.Facebook;
			SendToQueue(unifiedMessage);
		}

		[HttpPost]
		[Route("send/{facebookId}")]
		[FacebookMessageVerifier]
		public void Send([FromBody]UnifiedMessage message, string facebookId) {
			SendMessage(message, facebookId);
		}

		[HttpPost]
		[Route("channel/{facebookId}")]
		[FacebookMessageVerifier]
		public void ChannelSend([FromBody]UnifiedMessage message, string facebookId) {
			SendMessage(message, facebookId);
		}
	}
}