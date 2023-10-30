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

namespace UnusualSuspect.Api.Endpoints.User;

public sealed class GetProfileInfoEndpoint : MyBaseEndpointAuthenticated
.WithoutRequest
.WithActionResult<ApiResult<GetProfileInfoResponse>>
{
	private readonly IApplicationUserManager iApplicationUserManager;
	private readonly IDocumentService documentService;
	public GetProfileInfoEndpoint(IApplicationUserManager iApplicationUserManager, IDocumentService documentService)
	{
		this.iApplicationUserManager = iApplicationUserManager;
		this.documentService = documentService;
	}
	[HttpGet("api/[namespace]/GetProfileInfo")]
	public override async Task<ActionResult<ApiResult<GetProfileInfoResponse>>> HandleAsync(CancellationToken cancellationToken = default)
	{
		var user = await iApplicationUserManager.FindByNameAsync(CurrentUser.Username);
		if (user == null)
			return new ApiResult<GetProfileInfoResponse>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات کاربری یافت نشد!");
		Guid userImageDocumentGuidKey = Guid.Empty;
		if (user.DocumentId.HasValue)
		{
			var doc = await documentService.GetDocumentAsync(user.DocumentId.Value);
			if (doc == null)
			{
				ElmahExtensions.RaiseError(new Exception($"User has documentId but document do not exists. userId : {user.Id} - documentId: {user.DocumentId}"));
				return new ApiResult<GetProfileInfoResponse>(false, ApiResultStatusCode.LogicError, null,
					((int)LogicErrorCode.DocumentNotFound).ToString());
			}
			userImageDocumentGuidKey = doc.GuidKey;
		}
		return new ApiResult<GetProfileInfoResponse>(true, ApiResultStatusCode.Success, new GetProfileInfoResponse()
		{
			NickName = user.NickName,
			UserImageDocumentGuidKey = userImageDocumentGuidKey
		});
	}
}
