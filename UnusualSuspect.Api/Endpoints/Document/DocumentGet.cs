using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OutputCaching;
using UnusualSuspect.ApiViewModels.Endpoints.Document;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.IServices;

namespace UnusualSuspect.Api.Endpoints.Document;

public sealed class DocumentGet(IDocumentService documentService) : MyBaseEndpointAuthenticated
	.WithRequest<Guid>
	.WithActionResult<ApiResult<DocumentGetResponse>>
{
  [OutputCache(Duration = 60)]
	[HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
	public override async Task<ActionResult<ApiResult<DocumentGetResponse>>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		if(id == Guid.Empty)
			return new ApiResult<DocumentGetResponse>(false, ApiResultStatusCode.BadRequest, null, "کد فایل به درستی ارسال نشد");
		var doc = await documentService.GetDocumentByGuidKeyAsync(id);
		if (doc == null)
			return new ApiResult<DocumentGetResponse>(false, ApiResultStatusCode.NotFound, null, "فایل یافت نشد");
		return new ApiResult<DocumentGetResponse>(true, ApiResultStatusCode.Success, new DocumentGetResponse()
		{
			File = doc.File,
			FileName = doc.DocumentName
		});
	}
}