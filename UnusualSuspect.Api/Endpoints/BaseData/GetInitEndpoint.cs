using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Api.Endpoints.Account;
using UnusualSuspect.Common;

namespace UnusualSuspect.Api.Endpoints.BaseData
{
	public class GetInitEndpoint : EndpointBaseAsync
	.WithoutRequest
	.WithActionResult<ApiResult<GetInitResponse>>
	{
		[HttpGet("api/[namespace]/GetInit")]
		public override async Task<ActionResult<ApiResult<GetInitResponse>>> HandleAsync(CancellationToken cancellationToken)
		{
			return new ApiResult<GetInitResponse>(true, ApiResultStatusCode.Success, null, "دریافت اطلاعات اولیه");
		}
	}

	public class GetInitResponse
	{

	}
}
