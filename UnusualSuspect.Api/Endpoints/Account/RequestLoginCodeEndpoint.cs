using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Account;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Api.Endpoints.Account;

public class RequestLoginCodeEndpoint : EndpointBaseAsync
	.WithRequest<RequestLoginCodeRequest>
	.WithActionResult<ApiResult>
{
	private readonly IApplicationUserManager iApplicationUserManager;
	private readonly ISmsService smsService;
	private readonly IOptionsSnapshot<ProjectSetting> setting;
	private readonly ILogger<RequestLoginCodeEndpoint> logger;

	public RequestLoginCodeEndpoint(IApplicationUserManager iApplicationUserManager,
		IOptionsSnapshot<ProjectSetting> setting, ISmsService smsService, ILogger<RequestLoginCodeEndpoint> logger)
	{
		this.iApplicationUserManager = iApplicationUserManager;
		this.setting = setting ?? throw new ArgumentNullException(nameof(setting));
		this.smsService = smsService;
		this.logger = logger;
	}
	[AllowAnonymous]
	[HttpPost("api/[namespace]/RequestLoginCode")]
	public override async Task<ActionResult<ApiResult>> HandleAsync([FromBody] RequestLoginCodeRequest phoneNumber, CancellationToken cancellationToken = default)
	{
		//random delay
		await Task.Delay(new Random().Next(1, 500), cancellationToken);

		//logger.LogEvent(1, "نمونه لاگ information", 65, "this is for extra info");
		string phone = phoneNumber.KeyValue;
		if (!PhoneNumberHelper.CheckAndFixPhoneNumber(ref phone))
			return new ApiResult(false, ApiResultStatusCode.NeedToRetry
				, "لطفا شماره همراه خود را به درستی وارد نمایید");
		Random generator = new Random();
		string code = generator.Next(100000, 999999).ToString("D6");
		var user = await iApplicationUserManager.FindByNameAsync(phone);
		if (user != null)
		{
			user.PhoneNumberValidationCode = code;
			user.SendCodeDate = DateTime.Now;
			await iApplicationUserManager.UpdateAsync(user);
		}
		else
		{
			user = new Entities.Identity.ApplicationUser()
			{
				UserName = phone,
				PhoneNumber = phone,
				PhoneNumberConfirmed = false,
				DateCreate = DateTime.Now,
				IsActive = false,
				PhoneNumberValidationCode = code,
				SendCodeDate = DateTime.Now
			};
			await iApplicationUserManager.CreateAsync(user, Guid.NewGuid().ToString());
		}
		if (setting.Value.IsTesting)
			return new ApiResult(true, ApiResultStatusCode.Success, "کد تایید: " + code);
		if (await smsService.SendSmsAsync(phone,
			Common.Enums.SmsMessageTextEnum.LoginCodeSms, new List<string> { code }))
			return new ApiResult(true, ApiResultStatusCode.Success, "کد تایید به شماره همراه شما ارسال شد");
		return new ApiResult(false, ApiResultStatusCode.ServerError
			, "اشکالی در زمان ارسال کد به شماره همراه شما رخ داد. لطفا مجدد تلاش نمایید");
	}
}
