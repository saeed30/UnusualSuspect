using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public sealed class LeaveGameEndpoint(IGameService gameService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/LeaveGame")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<bool> game = await gameService.LeaveCurrentGameAsync(CurrentUser.UserId, cancellationToken);
    if (!game.Success)
      return new ApiResultCommon(false, ApiResultStatusCode.LogicError, game.MainError.ToString());
    await gameService.SaveChangesAsync(cancellationToken);
    return new ApiResultCommon(true, ApiResultStatusCode.Success);
  }
}