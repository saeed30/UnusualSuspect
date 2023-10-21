using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Common;
using Microsoft.AspNetCore.Http;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Services;
using UnusualSuspect.DataLayer;
using UnusualSuspect.ViewModels.Api.Endpoints.User;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class SaveProfileInfoEndpoint : EndpointBaseAsync
.WithRequest<SaveProfileInfoRequest>
.WithActionResult<ApiResult>
{
	private readonly IApplicationUserManager applicationUserManager;
	private readonly IApplicationUserService applicationUserService;
	private readonly IUnitOfWork _uow;

	public SaveProfileInfoEndpoint(IApplicationUserManager iApplicationUserManager, IApplicationUserService applicationUserService, IUnitOfWork uow)
	{
		this.applicationUserManager = iApplicationUserManager;
		this.applicationUserService = applicationUserService;
		_uow = uow;
	}
	[HttpPost("api/[namespace]/SaveProfileInfoEndpoint")]
	public override async Task<ActionResult<ApiResult>> HandleAsync(SaveProfileInfoRequest request, CancellationToken cancellationToken = default)
	{
		var user = await applicationUserManager.FindByNameAsync(HttpContext.User.Identity.Name);
		if (user == null)
			return new ApiResult(false, ApiResultStatusCode.NotFound, "اطلاعات کاربری یافت نشد!");
		user.NickName = request.NickName;
		user.ImageFile = request.UserImage;
		var result = await applicationUserService.EditApplicationUser(user, true);
		if(!result.Success)
			return new ApiResult(false, ApiResultStatusCode.ServerError, result.MessageList);
		await _uow.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}
