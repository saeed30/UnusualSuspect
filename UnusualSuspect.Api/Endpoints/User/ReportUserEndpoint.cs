using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.User;

public class ReportUserEndpoint(IReportUserService reportUserService, IUnitOfWork uow) : MyBaseEndpointAuthenticated
  .WithRequest<ReportUserRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/ReportUser")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(ReportUserRequest request, CancellationToken cancellationToken = default)
  {
    var result = await reportUserService.SaveReportAsync(CurrentUser.UserId, request, cancellationToken);
    if(!result.Success)
      return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
    if (result.Result)
      await uow.SaveChangesAsync(cancellationToken);
    return new ApiResultCommon(true, ApiResultStatusCode.Success);
  }
}