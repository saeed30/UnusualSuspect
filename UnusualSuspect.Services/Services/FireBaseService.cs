using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace UnusualSuspect.Services.Services;

public class FireBaseService(ILogger<FireBaseService> logger,
		IOptionsSnapshot<ProjectSetting> setting,
		IHttpContextAccessor contextAccessor)
	: IFireBaseService
{
	public async Task<HttpResponseMessage> SendNotification(string usertokenorchannelname, string message, string body, string subtitle)
	{
		try
		{
			HttpClient client = new HttpClient();
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
			requestMessage.Headers.TryAddWithoutValidation("Authorization", "key=" + setting.Value.FireBaseSetting.FireBaseToken);
			requestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json");
			requestMessage.Headers.TryAddWithoutValidation("Accept", "application/json");
			var JsonOB = new
			{
				to = usertokenorchannelname,
				priority = "high",
				notification = new
				{
					body = body,
					title = message,
					priority = "high",
					content_available = true,
					subtitle = subtitle,
				}
			};
			requestMessage.Content = new StringContent(JsonConvert.SerializeObject(JsonOB), Encoding.UTF8, "application/json");
			return await client.SendAsync(requestMessage);
		}
		catch
		{
			return null;
		}
	}

	public async Task<HttpResponseMessage> SendData(string usertokenorchannelname, object data)
	{
		try
		{
			HttpClient client = new HttpClient();
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
			requestMessage.Headers.TryAddWithoutValidation("Authorization", "key=" + setting.Value.FireBaseSetting.FireBaseToken);
			requestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json");
			requestMessage.Headers.TryAddWithoutValidation("Accept", "application/json");
			var JsonOB = new
			{
				to = usertokenorchannelname,
				priority = "high",
				data = data
			};
			requestMessage.Content = new StringContent(JsonConvert.SerializeObject(JsonOB), Encoding.UTF8, "application/json");
			return await client.SendAsync(requestMessage);
		}
		catch
		{
			return null;
		}
	}


	public async Task<HttpResponseMessage> SendFcm(string message, string body, string subtitle)
	{
		return await SendNotification(setting.Value.FireBaseSetting.FCMChannelName, message, body, subtitle);
	}

	public async Task SendGroupDataMessageAsync(IEnumerable<string> fireBasetokens, object p)
	{
		foreach (var item in fireBasetokens)
			await SendData(item, p);
	}
}
