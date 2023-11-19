using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game
{
	public sealed class GameGet(IGameService gameService) : MyBaseEndpointAuthenticated
		.WithRequest<int>
		.WithActionResult<ApiResult<GameGetResponse>>
	{
    [HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
		public async override Task<ActionResult<ApiResult<GameGetResponse>>> HandleAsync(int id, CancellationToken cancellationToken = default)
		{
			UnusualSuspectServiceResult<GameGetResponse> game = await gameService.GetGameAsync(id);
			if (!game.Success)
				return new ApiResult<GameGetResponse>(false, ApiResultStatusCode.LogicError, null, game.MainError.ToString());

			return new ApiResult<GameGetResponse>(true, ApiResultStatusCode.Success, game.Result);
		}
	}
}
