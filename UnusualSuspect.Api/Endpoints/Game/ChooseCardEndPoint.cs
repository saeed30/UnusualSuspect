using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Services;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game
{
  public class ChooseCardEndPoint(IGameService gameService) : MyBaseEndpointAuthenticated
    .WithRequest<FinishGameRequest>
    .WithActionResult<ApiResult<FinishGameResponse>>
  {
    public override async Task<ActionResult<ApiResult<FinishGameResponse>>> HandleAsync(FinishGameRequest request, CancellationToken cancellationToken = default)
    {
      UnusualSuspectServiceResult<bool?> result = await gameService.ChooseCardAndGetWinCondition(request.GameId, request.CardId, CurrentUser.UserId);
      if (!result.Success)
        return new ApiResult<FinishGameResponse>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
      await gameService.SaveChangesAsync(cancellationToken);
      return new ApiResult<FinishGameResponse>(true, ApiResultStatusCode.Success, new FinishGameResponse()
      {
        WonTheGame = result.Result
      });
    }
  }
}
