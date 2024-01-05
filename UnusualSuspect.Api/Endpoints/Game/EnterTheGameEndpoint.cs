using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Services;

namespace UnusualSuspect.Api.Endpoints.Game
{
  public class EnterTheGameEndpoint(IGameService gameService, INotificationService notificationService) : MyBaseEndpointAuthenticated
    .WithRequest<string>
    .WithActionResult<ApiResult>
  {
    [HttpGet("api/[namespace]/EnterTheGame")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(string connectionId, CancellationToken cancellationToken = new CancellationToken())
    {
      UnusualSuspectServiceResult<GameGetResponse> result = await gameService.GetCurrentGameAsync(CurrentUser.UserId, cancellationToken);
      if (!result.Success)
        return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
      if (result.Result == null)
        return new ApiResult(false, ApiResultStatusCode.NotFound, "هیچ بازی فعالی برای شما یافت نشد");
      await notificationService.AddToGroupAsync(CurrentUser.UserId, connectionId, result.Result.RoomName);
      return new ApiResult(true, ApiResultStatusCode.Success);

    }
  }
}
