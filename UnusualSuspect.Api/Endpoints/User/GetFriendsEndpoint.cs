using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class GetFriendsEndpoint(IFriendService friendService) : MyBaseEndpointAuthenticated
  .WithoutRequest
  .WithActionResult<ApiResultCommon<GetFriendsResponse>>
{
  [HttpGet("api/[namespace]/GetFriends")]
  public override async Task<ActionResult<ApiResultCommon<GetFriendsResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    var result = await friendService.GetFriendListAsync(CurrentUser.UserId, cancellationToken);
    return ReturnResult(result);
  }
}