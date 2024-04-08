using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public sealed class FinishedEndpoint(IGameService gameService) : MyBaseEndpointAuthenticated
  .WithRequest<int?>
  .WithActionResult<ApiResult<FinishedResponse>>
{
  [HttpGet("api/[namespace]/Finished")]
  public override async Task<ActionResult<ApiResult<FinishedResponse>>> HandleAsync(int? gameId, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<FinishedResponse> game = await gameService.GetFinishedResponseAsync(CurrentUser.UserId, gameId, cancellationToken);
    return ReturnResult(game);
  }
}