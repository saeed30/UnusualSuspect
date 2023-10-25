using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class GetProfileInfoEndpoint : MyBaseEndpointAuthenticated
.WithoutRequest
.WithActionResult<ApiResult<GetProfileInfoResponse>>
{
	private readonly IApplicationUserManager iApplicationUserManager;
	public GetProfileInfoEndpoint(IApplicationUserManager iApplicationUserManager)
	{
		this.iApplicationUserManager = iApplicationUserManager;
	}
	[HttpGet("api/[namespace]/GetProfileInfo")]
	public override async Task<ActionResult<ApiResult<GetProfileInfoResponse>>> HandleAsync(CancellationToken cancellationToken = default)
	{
		var user = await iApplicationUserManager.FindByNameAsync(CurrentUser.Username);
		if (user == null)
			return new ApiResult<GetProfileInfoResponse>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات کاربری یافت نشد!");
		return new ApiResult<GetProfileInfoResponse>(true, ApiResultStatusCode.Success, new GetProfileInfoResponse()
		{
			NickName = user.NickName,
			UserImageDocumentId = user.DocumentId
		});
	}
}
