using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using UnusualSuspect.Entities.Models;
using UnusualSuspect.DataLayer.Common;

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

	public async Task<bool> SendSmsAsync(string Mobile, string Message)
	{
		return true;
	}
}
