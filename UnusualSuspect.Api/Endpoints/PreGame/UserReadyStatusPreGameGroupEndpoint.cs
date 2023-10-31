using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;
public sealed class UserReadyStatusPreGameGroupEndpoint : MyBaseEndpointAuthenticated
	.WithRequest<UserReadyStatusPreGameGroupRequest>
	.WithActionResult<ApiResult>
{
	private readonly IPreGameService preGameService;
	public UserReadyStatusPreGameGroupEndpoint(IPreGameService preGameService)
	{
		this.preGameService = preGameService;
	}

	[HttpPost("api/[namespace]/UserReadyStatusPreGameGroup")]
	public override async Task<ActionResult<ApiResult>> HandleAsync([FromBody] UserReadyStatusPreGameGroupRequest request,
		CancellationToken cancellationToken = default)
	{
		if (!Enum.IsDefined(typeof(ReadyToGameStatusEnum), (int)request.ReadyToGameStatusId))
			return new ApiResult(false, ApiResultStatusCode.LogicError, ((int)LogicErrorCode.InvalidReadyToGameStatusId).ToString());
		ReadyToGameStatusEnum statusEnum = (ReadyToGameStatusEnum)request.ReadyToGameStatusId;
		var result = await preGameService.ChangeUserReadyStatus(CurrentUser.UserId, request.PreGameGroupId, statusEnum, cancellationToken);
		if (!result.Success)
			return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await preGameService.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}
