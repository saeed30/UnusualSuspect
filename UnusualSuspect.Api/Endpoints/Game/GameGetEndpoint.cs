using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public sealed class GameGetEndpoint(IGameService gameService) : MyBaseEndpointAuthenticated
  .WithRequest<int>
  .WithActionResult<ApiResult<GameGetResponse>>
{
  [HttpGet("api/[namespace]/GameGet")]
  public override async Task<ActionResult<ApiResult<GameGetResponse>>> HandleAsync(int gameId, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<GameGetResponse> game = await gameService.GetGameResponseAsync(CurrentUser.UserId, gameId, cancellationToken);
    return ReturnResult(game);
  }
}