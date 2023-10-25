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

public sealed class CreateGameGroupEndpoint : MyBaseEndpointAuthenticated
.WithRequest<CreateGameGroupRequest>
.WithActionResult<ApiResult<CreateGameGroupResponse>>
{
	private readonly IGameService gameService;
	public CreateGameGroupEndpoint(IGameService gameService)
	{
		this.gameService = gameService;
	}
	[HttpPost("api/[namespace]/CreateGameGroup")]
	public async override Task<ActionResult<ApiResult<CreateGameGroupResponse>>> HandleAsync([FromBody] CreateGameGroupRequest id, CancellationToken cancellationToken = default)
	{
		var result = await gameService.CreatePreGameGroup(CurrentUser.UserId, id.KeyValue);
		if(!result.Success)
			return new ApiResult<CreateGameGroupResponse>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());

		await gameService.SaveChangesAsync();
		return new ApiResult<CreateGameGroupResponse>(true, ApiResultStatusCode.Success, new CreateGameGroupResponse()
		{
			GameGroupId = result.Result.Id
		});
	}
}
