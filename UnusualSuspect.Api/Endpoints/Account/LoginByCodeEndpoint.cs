using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Account;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services.Contracts.Identity;
using Microsoft.Extensions.Logging;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;

namespace UnusualSuspect.Api.Endpoints.Account;

public class LoginByCodeEndpoint(IJwtService iJwtService,
    IApplicationUserManager iApplicationUserManager,
    ILogger<LoginByCodeEndpoint> logger)
  : EndpointBaseAsync
	.WithRequest<LoginByCodeRequest>
	.WithActionResult<ApiResultCommon<LoginByCodeRespond>>
{
  [AllowAnonymous]
	[HttpPost("api/[namespace]/LoginByCode")]
	public override async Task<ActionResult<ApiResultCommon<LoginByCodeRespond>>> HandleAsync([FromBody] LoginByCodeRequest loginByCodeRequest, CancellationToken cancellationToken = default)
	{
		//random delay
		var minWait = Task.Delay(new Random().Next(1, 2000), cancellationToken);

		string msg = ValidateLoginByCodeRequest(loginByCodeRequest);
    if (msg != null)
    {
      await minWait.ConfigureAwait(false);
			return new ApiResultCommon<LoginByCodeRespond>(false, ApiResultStatusCode.BadRequest, null, msg);
    }
		var user = await iApplicationUserManager.FindByNameAsync(loginByCodeRequest.Username);
    if (user == null)
    {
      await minWait.ConfigureAwait(false);
			return new ApiResultCommon<LoginByCodeRespond>(false, ApiResultStatusCode.NotFound, null, "کاربر مورد نظر یافت نشد!");
    }

    if (user.SendCodeDate.HasValue && user.SendCodeDate.Value.AddMinutes(5) < DateTime.Now)
    {
      await minWait.ConfigureAwait(false);
			return new ApiResultCommon<LoginByCodeRespond>(false, ApiResultStatusCode.BadRequest, null, "اعتبار کد تایید شما به پایان رسیده است. لطفا مجدد تلاش نمایید.");
    }

    if (user.PhoneNumberValidationCode != loginByCodeRequest.Code)
    {
      await minWait.ConfigureAwait(false);
			return new ApiResultCommon<LoginByCodeRespond>(false, ApiResultStatusCode.NeedToRetry, null, "کد وارد شده صحیح نیست");
    }
		var token = await iJwtService.GenerateAsync(user);
		user.PhoneNumberValidationCode = null;
		user.SendCodeDate = null;
		if (!user.PhoneNumberConfirmed)
		{
			user.PhoneNumberConfirmed = true;
			user.IsActive = true;
		}
		await iApplicationUserManager.UpdateLastLoginDateAsync(user);
    await minWait.ConfigureAwait(false);
		logger.LogEvent(SystemEventType.Login, user.Id, user.UserName);
		return new ApiResultCommon<LoginByCodeRespond>(true, ApiResultStatusCode.Success, new LoginByCodeRespond()
		{
			Access_token = token.access_token,
			Expires_in = token.expires_in,
			Refresh_token = token.refresh_token,
			Token_type = token.token_type,
			UserId = user.Id
		}, "ورود با موفقیت انجام شد");
	}

	private string ValidateLoginByCodeRequest(LoginByCodeRequest loginByCodeRequest)
	{
		if (loginByCodeRequest == null)
			return "اطلاعات درخواست ورود یافت نشد!";
		if (loginByCodeRequest.Username.IsNull())
			return "نام کاربری در درخواست کاربر یافت نشد.";
		if (loginByCodeRequest.Code.IsNull())
			return "کد تایید در درخواست کاربر یافت نشد.";
		string username = loginByCodeRequest.Username;
		if (!PhoneNumberHelper.CheckAndFixPhoneNumber(ref username))
			return "شماره همراه وارد شده معتبر نیست";
		loginByCodeRequest.Username = username;
		return null;
	}
}
