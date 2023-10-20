using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using UnusualSuspect.Api.Endpoints.User;
using UnusualSuspect.Common;
using UnusualSuspect.DataLayer.Common;
using Microsoft.AspNetCore.Http;
using UnusualSuspect.Services.IServices;

namespace UnusualSuspect.Api.Endpoints.Document
{
	public class Get : EndpointBaseAsync
	.WithRequest<int>
	.WithActionResult<ApiResult<DocumentGetResponse>>
	{
		private readonly IDocumentService documentService;
		public Get(IDocumentService documentService)
		{
			this.documentService = documentService;
		}
		[HttpGet("api/[namespace]/{id}", Name = "[namespace]_[controller]")]
		public override async Task<ActionResult<ApiResult<DocumentGetResponse>>> HandleAsync(int id, CancellationToken cancellationToken)
		{
			var doc = await documentService.GetDocumentAsync(id);
			if(doc == null)
				return new ApiResult<DocumentGetResponse>(false, ApiResultStatusCode.NotFound, null, "فایل یافت نشد");
			return new ApiResult<DocumentGetResponse>(true, ApiResultStatusCode.Success, new DocumentGetResponse()
			{
				File = doc.File,
				FileName = doc.DocumentName
			});
		}
	}
	public class DocumentGetResponse
	{
		public string FileName { get; set; }
		public byte[] File { get; set; }
	}
}
