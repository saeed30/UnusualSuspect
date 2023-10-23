using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Document;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.IServices;

namespace UnusualSuspect.Api.Endpoints.Document
{
	public class Get : EndpointBaseAsync
	.WithRequest<DocumentGetRequest>
	.WithActionResult<ApiResult<DocumentGetResponse>>
	{
		private readonly IDocumentService documentService;
		public Get(IDocumentService documentService)
		{
			this.documentService = documentService;
		}
		[HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
		public override async Task<ActionResult<ApiResult<DocumentGetResponse>>> HandleAsync(DocumentGetRequest id, CancellationToken cancellationToken)
		{
			var doc = await documentService.GetDocumentAsync(id.KeyValue);
			if (doc == null)
				return new ApiResult<DocumentGetResponse>(false, ApiResultStatusCode.NotFound, null, "فایل یافت نشد");
			return new ApiResult<DocumentGetResponse>(true, ApiResultStatusCode.Success, new DocumentGetResponse()
			{
				File = doc.File,
				FileName = doc.DocumentName
			});
		}
	}
}
