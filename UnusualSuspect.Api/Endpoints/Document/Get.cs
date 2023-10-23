using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Document;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.IServices;

namespace UnusualSuspect.Api.Endpoints.Document
{
	public class Get : EndpointBaseAsync
	.WithRequest<string>
	.WithActionResult<ApiResult<DocumentGetResponse>>
	{
		private readonly IDocumentService documentService;
		public Get(IDocumentService documentService)
		{
			this.documentService = documentService;
		}
		[HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
		public override async Task<ActionResult<ApiResult<DocumentGetResponse>>> HandleAsync(string id, CancellationToken cancellationToken)
		{
			int documentId;
			if(!int.TryParse(id, out documentId))
				return new ApiResult<DocumentGetResponse>(false, ApiResultStatusCode.BadRequest, null, "کد فایل به درستی ارسال نشد");
			var doc = await documentService.GetDocumentAsync(documentId);
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
