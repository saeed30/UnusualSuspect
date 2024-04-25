using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Utilities;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class AddUserToPreGameEndpoint(IPreGameService preGameService) : MyBaseEndpointAuthenticated
	.WithRequest<AddUserToPreGameRequest>
	.WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/AddUserToPreGame")]
	public override async Task<ActionResult<ApiResultCommon>> HandleAsync([FromBody] AddUserToPreGameRequest request,
		CancellationToken cancellationToken = default)
	{
		string phone = request.UserPhoneNumber;
		if (!PhoneNumberHelper.CheckAndFixPhoneNumber(ref phone))
			return new ApiResultCommon(false, ApiResultStatusCode.NeedToRetry
				, "لطفا شماره همراه خود را به درستی وارد نمایید");
		var result = await preGameService.AddUserToPreGameGroup(CurrentUser.UserId, phone, request.PreGameGroupId, cancellationToken);
		if (!result.Success)
			return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await preGameService.SaveChangesAsync(cancellationToken);
		return new ApiResultCommon(true, ApiResultStatusCode.Success);
	}
}