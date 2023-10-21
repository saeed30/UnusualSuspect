using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Api.Endpoints.User;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class GetProfileInfoEndpoint : EndpointBaseAsync
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
		var user = await iApplicationUserManager.FindByNameAsync(HttpContext.User.Identity.Name);
		if (user == null)
			return new ApiResult<GetProfileInfoResponse>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات کاربری یافت نشد!");
		return new ApiResult<GetProfileInfoResponse>(true, ApiResultStatusCode.Success, new GetProfileInfoResponse()
		{
			NickName = user.NickName,
			UserImageDocumentId = user.DocumentId
		});
	}
}
