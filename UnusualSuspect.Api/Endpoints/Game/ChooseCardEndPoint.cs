using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public class ChooseCardEndpoint(IGameService gameService, ITurnOfPlayService turnOfPlayService) : MyBaseEndpointAuthenticated
  .WithRequest<ChooseCardRequest>
  .WithActionResult<ApiResult<ChooseCardResponse>>
{
  [HttpPost("api/[namespace]/ChooseCard")]
  public override async Task<ActionResult<ApiResult<ChooseCardResponse>>> HandleAsync(ChooseCardRequest request, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<bool?> result = await gameService.ChooseCardAndGetWinCondition(request.GameId, request.CardId, CurrentUser.UserId, cancellationToken);
    if (!result.Success)
      return new ApiResult<ChooseCardResponse>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
    await gameService.GoToTalkingStatus(request.GameId);
    await gameService.SaveChangesAsync(cancellationToken);
    return new ApiResult<ChooseCardResponse>(true, ApiResultStatusCode.Success, new ChooseCardResponse()
    {
      WonTheGame = result.Result
    });
  }
}