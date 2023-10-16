using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Api.Endpoints.Account;
using UnusualSuspect.Common;
using Microsoft.AspNetCore.Authorization;

namespace UnusualSuspect.Api.Endpoints.BaseData
{
	public class GetInitEndpoint : EndpointBaseAsync
	.WithoutRequest
	.WithActionResult<ApiResult>
	{
		[Authorize]
		[HttpGet("api/[namespace]/GetInit")]
		public override async Task<ActionResult<ApiResult>> HandleAsync(CancellationToken cancellationToken)
		{
			return new ApiResult(true, ApiResultStatusCode.Success, "دریافت اطلاعات اولیه");
		}
	}
}
