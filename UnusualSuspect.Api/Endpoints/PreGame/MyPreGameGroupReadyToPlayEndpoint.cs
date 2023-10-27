using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class MyPreGameGroupReadyToPlayEndpoint : MyBaseEndpointAuthenticated
	.WithRequest<MyPreGameGroupReadyToPlayRequest>
	.WithActionResult<ApiResult>
{
	private readonly IGameService gameService;
	public MyPreGameGroupReadyToPlayEndpoint(IGameService gameService)
	{
		this.gameService = gameService;
	}

	[HttpPost("api/[namespace]/MyPreGameGroupReadyToPlay")]
	public override async Task<ActionResult<ApiResult>> HandleAsync(MyPreGameGroupReadyToPlayRequest request,
		CancellationToken cancellationToken = default)
	{
		var result = await gameService.PreGameReadyToPlayAsync(request.PreGameGroupId, cancellationToken);
		if (!result.Success)
			return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await gameService.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}