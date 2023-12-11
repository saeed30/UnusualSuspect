using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Identity;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class CreateGameGroupEndpoint(IPreGameService preGameService) : MyBaseEndpointAuthenticated
	.WithRequest<CreateGameGroupRequest>
	.WithActionResult<ApiResult<CreateGameGroupResponse>>
{
  [HttpPost("api/[namespace]/CreateGameGroup")]
	public override async Task<ActionResult<ApiResult<CreateGameGroupResponse>>> HandleAsync(
		[FromBody] CreateGameGroupRequest request, CancellationToken cancellationToken = default)
	{
		var result = await preGameService.CreatePreGameGroup(CurrentUser.UserId, request.GameTypeId, cancellationToken);
		if(!result.Success)
			return new ApiResult<CreateGameGroupResponse>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());

		await preGameService.SaveChangesAsync(cancellationToken);
		return new ApiResult<CreateGameGroupResponse>(true, ApiResultStatusCode.Success, new CreateGameGroupResponse()
		{
			GameGroupId = result.Result.Id
		});
	}
}
