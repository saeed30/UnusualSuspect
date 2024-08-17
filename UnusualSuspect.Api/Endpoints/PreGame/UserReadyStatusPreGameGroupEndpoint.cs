using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;
public sealed class UserReadyStatusPreGameGroupEndpoint(IPreGameService preGameService, INotificationService notificationService) : MyBaseEndpointAuthenticated
	.WithRequest<UserReadyStatusPreGameGroupRequest>
	.WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/UserReadyStatusPreGameGroup")]
	public override async Task<ActionResult<ApiResultCommon>> HandleAsync([FromBody] UserReadyStatusPreGameGroupRequest request,
		CancellationToken cancellationToken = default)
	{
		if (!Enum.IsDefined(typeof(ReadyToGameStatusEnum), (int)request.ReadyToGameStatusId))
			return new ApiResultCommon(false, ApiResultStatusCode.LogicError, ((int)LogicErrorCode.InvalidReadyToGameStatusId).ToString());
		ReadyToGameStatusEnum statusEnum = (ReadyToGameStatusEnum)request.ReadyToGameStatusId;
		var result = await preGameService.ChangeUserReadyStatusAsync(CurrentUser.UserId, request.PreGameGroupId, statusEnum, cancellationToken);
		if (!result.Success)
			return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await preGameService.SaveChangesAsync(cancellationToken);
    await notificationService.SendSignalToPreGameGroup(request.PreGameGroupId, SignalCommands.UserActiveStatusChangedInPregameGroup, 
      new { ReadyToGameStatusEnum = statusEnum , UserId = CurrentUser.UserId });

    return new ApiResultCommon(true, ApiResultStatusCode.Success);
	}
}
