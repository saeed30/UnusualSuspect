using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.LocalOnly;

public class ChangeGameStatusEndpoint(ILogger<ChangeGameStatusEndpoint> logger,
  IGameService gameService) : MyBaseEndpointLocal
  .WithRequest<ChangeGameStateRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/ChangeGameState")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync([FromBody] ChangeGameStateRequest request, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<bool>  result = await gameService.ManualSetGameStatusAsync(request.GameId, request.GameStatus, cancellationToken);
    if(!result.Success)
      return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.GetDisplay());
    await gameService.SaveChangesAsync(cancellationToken);
    return new ApiResultCommon(true, ApiResultStatusCode.Success);
  }
}