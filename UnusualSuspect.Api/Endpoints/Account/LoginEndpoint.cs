using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Common;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Identity;

namespace UnusualSuspect.Api.Endpoints.Account
{
	public class LoginEndpoint : EndpointBaseAsync
		.WithRequest<LoginRequest>
		.WithActionResult<ApiResult<AccessToken>>
	{
		private readonly IJwtService iJwtService;
		private readonly IApplicationUserManager iApplicationUserManager;


		public LoginEndpoint(IJwtService iJwtService, IApplicationUserManager iApplicationUserManager)
		{
			this.iJwtService = iJwtService;
			this.iApplicationUserManager = iApplicationUserManager;
		}

		[HttpPost("api/[namespace]/Login")]
		public override async Task<ActionResult<ApiResult<AccessToken>>> HandleAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
		{
			var user = iApplicationUserManager.FindByName(request.Username);
			if (user == null)
				return new ApiResult<AccessToken>(false, ApiResultStatusCode.Success, null, "کاربر مورد نظر یافت نشد!");
			var token = await iJwtService.GenerateAsync(user);
			return new ApiResult<AccessToken>(true, ApiResultStatusCode.Success, token, "ورود با موفقیت انجام شد");
		}
	}
}
