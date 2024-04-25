using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public class ChooseCardEndpoint(IGameService gameService, INotificationService notificationService,
  IMemoryCacheService memoryCacheService) : MyBaseEndpointAuthenticated
  .WithRequest<ChooseCardRequest>
  .WithActionResult<ApiResultCommon<ChooseCardResponse>>
{
  [HttpPost("api/[namespace]/ChooseCard")]
  public override async Task<ActionResult<ApiResultCommon<ChooseCardResponse>>> HandleAsync(ChooseCardRequest request, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<bool?> result = await gameService.ChooseCardAndGetWinCondition(request.GameId, request.CardId, CurrentUser.UserId, cancellationToken);
    if (!result.Success)
      return new ApiResultCommon<ChooseCardResponse>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
    await gameService.SaveChangesAsync(cancellationToken);
		memoryCacheService.ClearGameWithDetails(request.GameId);
    if (result.Result.HasValue)
      await notificationService.RemoveAllUsersFromGame(request.GameId);
    return new ApiResultCommon<ChooseCardResponse>(true, ApiResultStatusCode.Success, new ChooseCardResponse()
    {
      WonTheGame = result.Result
    });
  }
}