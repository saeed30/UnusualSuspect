using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Options;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;

namespace UnusualSuspect.Services.Services;

public class SmsService(IOptionsSnapshot<ProjectSetting> setting, ISmsLogRepository smsLogRepository)
  : ISmsService
{
  public SmsLog SaveSmsSentLog(SmsLog smsLog)
	{
		return smsLogRepository.Add(smsLog);
	}

	public async Task<bool> SendSmsAsync(string Mobile, SmsMessageTextEnum message, List<string> parameters = null)
	{
		string msg = GetMessageFromEnum(message, parameters);
		return await SendSmsAsync(Mobile, msg);
	}

	public async Task<bool> SendSmsAsync(string Mobile, string Message)
	{
		return true;
	}
	private string GetMessageFromEnum(SmsMessageTextEnum message, List<string> parameters)
	{
		string msg = message.ToDisplay();
		if (parameters == null || parameters.Count == 0)
			return msg;
		return string.Format(msg, parameters);
	}
}
