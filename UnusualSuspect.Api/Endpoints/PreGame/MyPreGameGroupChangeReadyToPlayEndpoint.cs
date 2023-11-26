using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class MyPreGameGroupChangeReadyToPlayEndpoint(IPreGameService preGameService) : MyBaseEndpointAuthenticated
	.WithRequest<MyPreGameGroupChangeReadyToPlayRequest>
	.WithActionResult<ApiResult>
{
  [HttpPost("api/[namespace]/MyPreGameGroupChangeReadyToPlay")]
	public override async Task<ActionResult<ApiResult>> HandleAsync([FromBody] MyPreGameGroupChangeReadyToPlayRequest request,
		CancellationToken cancellationToken = default)
	{
		PreGameGroupStatusEnum preGameGroupStatusEnum = request.IsReady ? PreGameGroupStatusEnum.Ready : PreGameGroupStatusEnum.NotReady;
		var result = await preGameService.PreGameGroupChangeReadyToPlayAsync(request.PreGameGroupId, preGameGroupStatusEnum, cancellationToken);
		if (!result.Success)
			return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
		await preGameService.SaveChangesAsync(cancellationToken);
		return new ApiResult(true, ApiResultStatusCode.Success);
	}
}