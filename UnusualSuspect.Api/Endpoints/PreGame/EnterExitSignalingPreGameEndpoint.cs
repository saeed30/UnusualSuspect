using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public class EnterExitSignalingPreGameEndpoint(IPreGameService preGameService, INotificationService notificationService) : MyBaseEndpointAuthenticated
  .WithRequest<EnterExitSignalingPreGameRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/EnterExitSignalingPreGame")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(EnterExitSignalingPreGameRequest request, CancellationToken cancellationToken = default)
  {
    if (request.ConnectionId.IsNull())
      return new BadRequestResult();
    int userId = CurrentUser.UserId;
		if (request.IsEntering)
    {
      UnusualSuspectServiceResult<bool> result = await preGameService.IsMemberOfPregameGroup(userId, request.PreGameGroupId, cancellationToken);
      if(!result.Success)
        return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
      if(!result.Result)
        return new ApiResultCommon(false, ApiResultStatusCode.LogicError, LogicErrorCode.UserNotMemberOfPreGameGroup.ToString());
      await notificationService.AddToGroupAsync(userId, request.ConnectionId, "pre" + request.PreGameGroupId);
    }
    else
    {
      await notificationService.RemoveFromGroupAsync(userId, "pre" + request.PreGameGroupId, request.ConnectionId);
    }
    return new OkResult();
  }
}