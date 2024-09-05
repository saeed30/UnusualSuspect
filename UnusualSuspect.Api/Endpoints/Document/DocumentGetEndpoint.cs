using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OutputCaching;
using UnusualSuspect.ApiViewModels.Endpoints.Document;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.IServices;
using UnusualSuspect.Services;

namespace UnusualSuspect.Api.Endpoints.Document;

public sealed class DocumentGetEndpoint(IDocumentService documentService) : MyBaseEndpointAuthenticated
	.WithRequest<Guid>
	.WithActionResult<ApiResultCommon<DocumentGetResponse>>
{
  [OutputCache(Duration = 60)]
	[HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
	public override async Task<ActionResult<ApiResultCommon<DocumentGetResponse>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		if(id == Guid.Empty)
			return new ApiResultCommon<DocumentGetResponse>(false, ApiResultStatusCode.BadRequest, null, "کد فایل به درستی ارسال نشد");
    UnusualSuspectServiceResult<DocumentGetResponse> doc = await documentService.GetDocumentByGuidKeyAsync(id);
    return ReturnResult(doc);
	}
}