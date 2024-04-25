using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.User;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.IServices;
using ElmahCore;
using UnusualSuspect.Entities.Dtos;
using UnusualSuspect.Services;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class GetProfileInfoEndpoint(
    IApplicationUserManager iApplicationUserManager,
    IDocumentService documentService,
    IGameService gameService)
  : MyBaseEndpointAuthenticated
.WithoutRequest
.WithActionResult<ApiResultCommon<GetProfileInfoResponse>>
{
  [HttpGet("api/[namespace]/GetProfileInfo")]
  public override async Task<ActionResult<ApiResultCommon<GetProfileInfoResponse>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    var user = await iApplicationUserManager.FindByNameAsync(CurrentUser.Username);
    if (user == null)
      return new ApiResultCommon<GetProfileInfoResponse>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات کاربری یافت نشد!");
    Guid? userImageDocumentGuidKey = null;
    if (user.DocumentId.HasValue)
    {
      var doc = await documentService.GetDocumentAsync(user.DocumentId.Value);
      if (doc == null)
      {
        ElmahExtensions.RaiseError(new Exception($"User has documentId but document do not exists. userId : {user.Id} - documentId: {user.DocumentId}"));
        return new ApiResultCommon<GetProfileInfoResponse>(false, ApiResultStatusCode.LogicError, null,
          ((int)LogicErrorCode.DocumentNotFound).ToString());
      }
      userImageDocumentGuidKey = doc.GuidKey;
    }

    UnusualSuspectServiceResult<GameStatisticsDto> gameStatistics = await gameService.GetGameStatisticsAsync(CurrentUser.UserId, cancellationToken);
    if (!gameStatistics.Success)
      return new ApiResultCommon<GetProfileInfoResponse>(false, ApiResultStatusCode.LogicError, null, gameStatistics.MainError.ToString());
    return new ApiResultCommon<GetProfileInfoResponse>(true, ApiResultStatusCode.Success, new GetProfileInfoResponse()
    {
      NickName = user.NickName,
      UserImageDocumentGuidKey = userImageDocumentGuidKey == null ? null : userImageDocumentGuidKey.ToString(),
      AvatarId = user.AvatarId.HasValue ? user.AvatarId.Value : -1,
      IsMale = user.IsMale.HasValue ? user.IsMale.Value : true,
      Ranking = user.Ranking ?? 10042,
      GamesLost = gameStatistics.Result.GamesLost,
      GamesPlayed = gameStatistics.Result.GamesPlayed,
      GamesWon = gameStatistics.Result.GamesWon,
      UserId = user.Id
    });
  }
}
