using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class MyPreGameGroupReadyToPlayEndpoint : MyBaseEndpointAuthenticated
	.WithRequest<MyPreGameGroupReadyToPlayRequest>
	.WithActionResult<ApiResult>
{
	private readonly IGameService _gameService;
	public MyPreGameGroupReadyToPlayEndpoint(IGameService gameService)
	{
		this._gameService = gameService;
	}

	[HttpPost("api/[namespace]/MyPreGameGroupReadyToPlay")]
	public override async Task<ActionResult<ApiResult>> HandleAsync(MyPreGameGroupReadyToPlayRequest request,
		CancellationToken cancellationToken = default)
	{
		throw new System.NotImplementedException();
	}
}