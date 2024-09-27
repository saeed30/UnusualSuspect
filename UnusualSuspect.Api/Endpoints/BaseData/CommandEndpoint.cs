using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Models;
using UnusualSuspect.DataLayer.Contracts;

namespace UnusualSuspect.Api.Endpoints.BaseData
{
  public sealed class CommandEndpoint(ILogger<CommandEndpoint> logger, IMemoryCacheService memoryCacheService) : MyBaseEndpointAuthenticated
    .WithRequest<string>
    .WithActionResult<ApiResultCommon>
  {
    [HttpGet("api/[namespace]/Command")]
    public override async Task<ActionResult<ApiResultCommon>> HandleAsync(string request, CancellationToken cancellationToken = new CancellationToken())
    {
      var minWait = Task.Delay(new Random().Next(1, 2000), cancellationToken);
      bool hasMatch = false;
      switch (request)
      {
        case "whorunsbartertown":
          hasMatch = true;
          logger.LogEvent(SystemEventType.ManualClearSoftSettingCache, CurrentUser.UserId);
          memoryCacheService.ClearSoftSetting();
          break;
      }
      await minWait.ConfigureAwait(false);
      return new ApiResultCommon(hasMatch, ApiResultStatusCode.Success);
    }
  }
}
