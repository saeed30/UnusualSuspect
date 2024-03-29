using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Game;

public class EnterTheGameEndpoint(IGameService gameService, INotificationService notificationService) : MyBaseEndpointAuthenticated
  .WithRequest<EnterTheGameRequest>
  .WithActionResult<ApiResult<GameGetResponse>>
{
  [HttpGet("api/[namespace]/EnterTheGame")]
  public override async Task<ActionResult<ApiResult<GameGetResponse>>> HandleAsync(
    EnterTheGameRequest request, CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<GameGetResponse> result = await gameService.GetGameResponseAsync(CurrentUser.UserId, request.GameId, cancellationToken);
    if (!result.Success)
      return new ApiResult<GameGetResponse>(false, ApiResultStatusCode.LogicError, null, result.MainError.ToString());
    if (result.Result == null)
      return new ApiResult<GameGetResponse>(false, ApiResultStatusCode.NotFound, null, "هیچ بازی فعالی برای شما یافت نشد");
    await notificationService.AddToGroupAsync(CurrentUser.UserId, request.ConnectionId, result.Result.RoomName);
    return new ApiResult<GameGetResponse>(true, ApiResultStatusCode.Success, result.Result);
  }
}