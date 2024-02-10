using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame
{
  public class EnterExitSignalingPreGameEndpoint(IPreGameService preGameService, INotificationService notificationService) : MyBaseEndpointAuthenticated
    .WithRequest<EnterExitSignalingPreGameRequest>
    .WithActionResult<ApiResult>
  {
    [HttpPost("api/[namespace]/EnterExitSignalingPreGame")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(EnterExitSignalingPreGameRequest request, CancellationToken cancellationToken = default)
    {
      if (request.ConnectionId.IsNull())
        return new BadRequestResult();
      if (request.IsEntering)
      {
        UnusualSuspectServiceResult<bool> result = await preGameService.IsMemberOfPregameGroup(CurrentUser.UserId, request.PreGameGroupId, cancellationToken);
        if(!result.Success)
          return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
        if(!result.Result)
          return new ApiResult(false, ApiResultStatusCode.LogicError, LogicErrorCode.UserNotMemberOfPreGameGroup.ToString());
        await notificationService.AddToGroupAsync(CurrentUser.UserId, request.ConnectionId, "pre" + request.PreGameGroupId);
      }
      else
      {
        await notificationService.RemoveFromGroupAsync(CurrentUser.UserId, "pre" + request.PreGameGroupId, request.ConnectionId);
      }
      return new OkResult();
    }
  }
}
