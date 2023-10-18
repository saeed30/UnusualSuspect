using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.DataLayer.Common;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;

namespace UnusualSuspect.Services;

public class SmsService : ISmsService
{
	private readonly IOptionsSnapshot<ProjectSetting> _setting;
	private readonly IAsyncRepository<SmsLog> asyncRepository;

	public SmsService(
			IOptionsSnapshot<ProjectSetting> setting, IAsyncRepository<SmsLog> asyncRepository)
	{
		_setting = setting ?? throw new ArgumentNullException(nameof(setting));
		this.asyncRepository = asyncRepository;
	}

	public async Task<SmsLog> SaveSmsSentLog(SmsLog smsLog, CancellationToken cancellationToken)
	{
		return await asyncRepository.AddAsync(smsLog, cancellationToken);
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
