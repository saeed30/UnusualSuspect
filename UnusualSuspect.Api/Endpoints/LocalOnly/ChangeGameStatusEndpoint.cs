using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.LocalOnly;
using UnusualSuspect.Common.Attribute;
using UnusualSuspect.Common.Models;

namespace UnusualSuspect.Api.Endpoints.LocalOnly;

public class ChangeGameStatusEndpoint(ILogger<ChangeGameStatusEndpoint> logger) : MyBaseEndpointAuthenticated
  .WithRequest<ChangeGameStateRequest>
  .WithActionResult<ApiResultCommon>
{
  [AllowAnonymous]
  [LocalRequestOnly]
  [ApiExplorerSettings(IgnoreApi = true)]
  [HttpPost("api/[namespace]/ChangeGameState")]
  public override async Task<ActionResult<ApiResultCommon>> HandleAsync(ChangeGameStateRequest request, CancellationToken cancellationToken = default)
  {
    throw new System.NotImplementedException();
  }
}