using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class ReportUserTypesGetEndpoint(IReportUserService reportUserService)
  : MyBaseEndpointAuthenticated
    .WithoutRequest
    .WithActionResult<ApiResultCommon<ReportUserTypesGetResponse>>
{
  [HttpGet("api/[namespace]/ReportUserTypesGet")]
  public override async Task<ActionResult<ApiResultCommon<ReportUserTypesGetResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    UnusualSuspectServiceResult<ReportUserTypesGetResponse> result = await reportUserService.GetAllReportUserTypes(cancellationToken);
    return ReturnResult(result);
  }
}