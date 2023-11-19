using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class SaveProfileInfoEndpoint(IApplicationUserManager iApplicationUserManager,
    IApplicationUserService applicationUserService, IUnitOfWork uow)
  : MyBaseEndpointAuthenticated
.WithRequest<SaveProfileInfoRequest<IFormFile>>
.WithActionResult<ApiResult>
{
  [HttpPost("api/[namespace]/SaveProfileInfoEndpoint")]
	public override async Task<ActionResult<ApiResult>> HandleAsync(SaveProfileInfoRequest<IFormFile> request, CancellationToken cancellationToken = default)
	{
		var user = await iApplicationUserManager.FindByNameAsync(CurrentUser.Username);
		if (user == null)
			return new ApiResult(false, ApiResultStatusCode.NotFound, "اطلاعات کاربری یافت نشد!");
		user.NickName = request.NickName;
		user.ImageFile = request.UserImage;
		var result = await applicationUserService.EditApplicationUser(user, true);
		if (!result.Success)
			return new ApiResult(false, ApiResultStatusCode.ServerError, result.MessageList);
		await uow.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}
