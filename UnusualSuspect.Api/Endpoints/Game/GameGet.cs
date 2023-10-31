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
	public sealed class GameGet : MyBaseEndpointAuthenticated
		.WithRequest<int>
		.WithActionResult<ApiResult<GameGetResponse>>
	{
		private readonly IGameService gameService;
		public GameGet(IGameService gameService)
		{
			this.gameService = gameService;
		}

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
