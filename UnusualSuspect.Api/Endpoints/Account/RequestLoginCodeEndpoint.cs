using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Common;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Identity;
using UnusualSuspect.Common.Utilities;
using Microsoft.Extensions.Options;
using UnusualSuspect.ViewModels.Settings;
using System;
using System.Reflection.Emit;
using UnusualSuspect.Services.Contracts;
using System.Collections.Generic;

namespace UnusualSuspect.Api.Endpoints.Account;

public class RequestLoginCodeEndpoint : EndpointBaseAsync
	.WithRequest<string>
	.WithActionResult<ApiResult<string>>
{
	private readonly IJwtService iJwtService;
	private readonly IApplicationUserManager iApplicationUserManager;
	private readonly ISmsService smsService;
	private readonly IOptionsSnapshot<ProjectSetting> setting;

	public RequestLoginCodeEndpoint(IJwtService iJwtService, IApplicationUserManager iApplicationUserManager,
		IOptionsSnapshot<ProjectSetting> setting, ISmsService smsService)
	{
		this.iJwtService = iJwtService;
		this.iApplicationUserManager = iApplicationUserManager;
		this.setting = setting ?? throw new ArgumentNullException(nameof(setting));
		this.smsService = smsService;
	}

	[HttpPost("api/[namespace]/RequestLoginCode")]
	public override async Task<ActionResult<ApiResult<string>>> HandleAsync([FromBody] string phoneNumber, CancellationToken cancellationToken)
	{
		if(!PhoneNumberHelper.CheckAndFixPhoneNumber(ref phoneNumber))
			return new ApiResult<string>(false, ApiResultStatusCode.NeedToRetry, ""
				, "لطفا شماره همراه خود را به درستی وارد نمایید");
		Random generator = new Random();
		string code = generator.Next(100000, 999999).ToString("D6");
		var user = iApplicationUserManager.FindByName(phoneNumber);
		if(user != null)
		{
			user.PhoneNumberValidationCode = code;
			user.SendCodeDate = DateTime.Now;
			await iApplicationUserManager.UpdateAsync(user);
		}
		else
		{
			user = new Entities.Identity.ApplicationUser()
			{
				UserName = phoneNumber,
				PhoneNumber = phoneNumber,
				PhoneNumberConfirmed = false,
				DateCreate = DateTime.Now,
				IsActive = false,
				PhoneNumberValidationCode = code,
				SendCodeDate = DateTime.Now
			};
			await iApplicationUserManager.CreateAsync(user, Guid.NewGuid().ToString());
		}
		if (setting.Value.IsTesting)
			return new ApiResult<string>(true, ApiResultStatusCode.Success, code, "ورود با موفقیت انجام شد");
		if (await smsService.SendSmsAsync(phoneNumber,
			Common.Enums.SmsMessageTextEnum.LoginCodeSms, new List<string> { code }))
			return new ApiResult<string>(true, ApiResultStatusCode.Success, "", "کد تایید به شماره همراه شما ارسال شد");
		return new ApiResult<string>(false, ApiResultStatusCode.ServerError, ""
			, "اشکالی در زمان ارسال کد به شماره همراه شما رخ داد. لطفا مجدد تلاش نمایید");
	}
}
