using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public sealed class TurnOfPlayGetEndpoint(ITurnOfPlayService turnOfPlayService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResult<TurnOfPlayGetResponse>>
{
  [HttpGet("api/[namespace]/TurnOfPlayGet")]
  public override async Task<ActionResult<ApiResult<TurnOfPlayGetResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<TurnOfPlayGetResponse> game = await turnOfPlayService.GetTurnOfPlayGetAsync(CurrentUser.UserId, cancellationToken);
    return ReturnResult(game);
  }
}