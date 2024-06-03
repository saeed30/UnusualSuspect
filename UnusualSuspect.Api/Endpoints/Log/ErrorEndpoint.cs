using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.Log;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;

namespace UnusualSuspect.Api.Endpoints.Log;

public sealed class ErrorEndpoint(ILogger<ErrorEndpoint> logger) : MyBaseEndpointAuthenticated
  .WithRequest<ErrorRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/Error")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(ErrorRequest request, CancellationToken cancellationToken = default)
  {
    if (request == null || string.IsNullOrWhiteSpace(request.ErrorContent))
      return new ApiResultCommon(false, ApiResultStatusCode.BadRequest);
    logger.LogError(request.ErrorContent, request.ErrorParameterList);
    return new ApiResultCommon(true, ApiResultStatusCode.Success);
  }
}