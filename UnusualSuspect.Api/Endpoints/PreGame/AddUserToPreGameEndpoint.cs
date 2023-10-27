using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Utilities;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class AddUserToPreGameEndpoint : MyBaseEndpointAuthenticated
	.WithRequest<AddUserToPreGameRequest>
	.WithActionResult<ApiResult>
{
	private readonly IGameService gameService;
	public AddUserToPreGameEndpoint(IGameService gameService)
	{
		this.gameService = gameService;
	}
	[HttpPost("api/[namespace]/AddUserToPreGame")]
	public override async Task<ActionResult<ApiResult>> HandleAsync([FromBody] AddUserToPreGameRequest request,
		CancellationToken cancellationToken = default)
	{
		string phone = request.UserPhoneNumber;
		if (!PhoneNumberHelper.CheckAndFixPhoneNumber(ref phone))
			return new ApiResult(false, ApiResultStatusCode.NeedToRetry
				, "لطفا شماره همراه خود را به درستی وارد نمایید");
		var result = await gameService.AddUserToPreGameGroup(CurrentUser.UserId, phone, request.PreGameGroupId, cancellationToken);
		if (!result.Success)
			return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await gameService.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}