using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels;
using UnusualSuspect.ApiViewModels.Endpoints.Log;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.Api.Endpoints.Log
{
  public sealed class ErrorEndpoint(ILogger<ErrorEndpoint> logger) : MyBaseEndpointAuthenticated
    .WithRequest<ErrorRequest>
    .WithActionResult<ApiResult>
  {
    [HttpPost("api/[namespace]/Error")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(ErrorRequest request, CancellationToken cancellationToken = default)
    {
      if (request == null || string.IsNullOrWhiteSpace(request.ErrorContent))
        return new ApiResult(false, ApiResultStatusCode.BadRequest);
      logger.LogError(request.ErrorContent);
      return new ApiResult(true, ApiResultStatusCode.Success);
    }
  }
}
