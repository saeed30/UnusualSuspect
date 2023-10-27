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
	private readonly IGameService _gameService;
	public UserReadyStatusPreGameGroupEndpoint(IGameService gameService)
	{
		this._gameService = gameService;
	}

	public override async Task<ActionResult<ApiResult>> HandleAsync(UserReadyStatusPreGameGroupRequest request,
		CancellationToken cancellationToken = new CancellationToken())
	{
		if(!Enum.IsDefined(typeof(ReadyToGameStatusEnum), request.ReadyToGameStatusId))
			return new ApiResult(false, ApiResultStatusCode.LogicError, ((int)LogicErrorCode.InvalidReadyToGameStatusId).ToString());
		ReadyToGameStatusEnum statusEnum = (ReadyToGameStatusEnum)request.ReadyToGameStatusId;
		var result = await _gameService.ChangeUserReadyStatus(CurrentUser.UserId, request.PreGameGroupId, statusEnum, cancellationToken);
		if(!result.Success)
			return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await _gameService.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}
