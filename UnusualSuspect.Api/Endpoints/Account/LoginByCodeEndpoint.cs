using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Api.Endpoints.Account;
using UnusualSuspect.ViewModels.Identity;

namespace UnusualSuspect.Api.Endpoints.Account;

public class LoginByCodeEndpoint : EndpointBaseAsync
	.WithRequest<LoginByCodeRequest>
	.WithActionResult<ApiResult<AccessToken>>
{
	private readonly IJwtService iJwtService;
	private readonly IApplicationUserManager iApplicationUserManager;

	public LoginByCodeEndpoint(IJwtService iJwtService, IApplicationUserManager iApplicationUserManager)
	{
		this.iJwtService = iJwtService;
		this.iApplicationUserManager = iApplicationUserManager;
	}
	[AllowAnonymous]
	[HttpPost("api/[namespace]/LoginByCode")]
	public override async Task<ActionResult<ApiResult<AccessToken>>> HandleAsync([FromBody] LoginByCodeRequest loginByCodeRequest, CancellationToken cancellationToken)
	{
		string msg = ValidateLoginByCodeRequest(loginByCodeRequest);
		if (msg != null)
			return new ApiResult<AccessToken>(false, ApiResultStatusCode.BadRequest, null, msg);
		var user = await iApplicationUserManager.FindByNameAsync(loginByCodeRequest.Username);
		if (user == null)
			return new ApiResult<AccessToken>(false, ApiResultStatusCode.NotFound, null, "کاربر مورد نظر یافت نشد!");
		if (user.SendCodeDate.HasValue && user.SendCodeDate.Value.AddMinutes(5) < DateTime.Now)
			return new ApiResult<AccessToken>(false, ApiResultStatusCode.BadRequest, null, "اعتبار کد تایید شما به پایان رسیده است. لطفا مجدد تلاش نمایید.");
		if (user.PhoneNumberValidationCode != loginByCodeRequest.Code)
			return new ApiResult<AccessToken>(false, ApiResultStatusCode.NeedToRetry, null, "کد وارد شده صحیح نیست");
		var token = await iJwtService.GenerateAsync(user);
		user.PhoneNumberValidationCode = null;
		user.SendCodeDate = null;
		if (!user.PhoneNumberConfirmed)
		{
			user.PhoneNumberConfirmed = true;
			user.IsActive = true;
		}
		await iApplicationUserManager.UpdateLastLoginDateAsync(user);
		return new ApiResult<AccessToken>(true, ApiResultStatusCode.Success, token, "ورود با موفقیت انجام شد");
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
