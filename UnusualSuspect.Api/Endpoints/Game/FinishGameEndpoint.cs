using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public sealed class FinishGameEndpoint(IGameService gameService) : MyBaseEndpointAuthenticated
  .WithRequest<int>
  .WithActionResult<ApiResult>
{
  public override async Task<ActionResult<ApiResult>> HandleAsync(int id, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<bool> game = await gameService.FinishGameAsync(id, CurrentUser.UserId);
    if (!game.Success)
      return new ApiResult(false, ApiResultStatusCode.LogicError, game.MainError.ToString());
    await gameService.SaveChangesAsync(cancellationToken);
    return new ApiResult(true, ApiResultStatusCode.Success);
  }
}