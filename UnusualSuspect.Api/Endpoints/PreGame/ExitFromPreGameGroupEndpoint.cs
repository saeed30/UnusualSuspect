using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;
public sealed class ExitFromPreGameGroupEndpoint(IPreGameService preGameService) : MyBaseEndpointAuthenticated
	.WithRequest<ExitFromPreGameGroupRequest>
	.WithActionResult<ApiResult>
{
	public override async Task<ActionResult<ApiResult>> HandleAsync(ExitFromPreGameGroupRequest request, CancellationToken cancellationToken = default)
	{
		UnusualSuspectServiceResult<bool> result = await preGameService.ExitFromPreGameGroup(request.PreGameGroupId, request.UserId, CurrentUser.UserId, cancellationToken);
		if (!result.Success)
			return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		if (result.Result)
			await preGameService.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}
