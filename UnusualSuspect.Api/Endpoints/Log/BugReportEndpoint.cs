using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.Log;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.Log;

public sealed class BugReportEndpoint(ILogger<BugReportEndpoint> logger,
  IMessageService messageService, IUnitOfWork uow,
  INotificationService notificationService) : MyBaseEndpointAuthenticated
  .WithRequest<BugReportRequest>
  .WithActionResult<ApiResultCommon>
{
  [HttpPost("api/[namespace]/BugReport")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(BugReportRequest request, CancellationToken cancellationToken = default)
  {
    if (request == null || string.IsNullOrWhiteSpace(request.UserDesc))
      return new ApiResultCommon(false, ApiResultStatusCode.BadRequest);
    var result = messageService.SendMessageToAdmin("BugReport: " + request.UserDesc, CurrentUser.UserId);
    if (!result.Success)
      return new ApiResultCommon(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
    await uow.SaveChangesAsync(cancellationToken);
    return new ApiResultCommon(true, ApiResultStatusCode.Success);
  }
}