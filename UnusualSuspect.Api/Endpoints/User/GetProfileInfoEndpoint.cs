using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Api.Endpoints.Account;
using UnusualSuspect.Common;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Api.Endpoints.User
{
	public class GetProfileInfoEndpoint : EndpointBaseAsync
	.WithoutRequest
	.WithActionResult<ApiResult<GetProfileInfoResponse>>
	{
		private readonly IApplicationUserManager iApplicationUserManager;
		public GetProfileInfoEndpoint(IApplicationUserManager iApplicationUserManager)
		{
			this.iApplicationUserManager = iApplicationUserManager;
		}
		[HttpPost("api/[namespace]/GetProfileInfo")]
		public override async Task<ActionResult<ApiResult<GetProfileInfoResponse>>> HandleAsync(CancellationToken cancellationToken = default)
		{
			var user = await iApplicationUserManager.FindByNameAsync(HttpContext.User.Identity.Name);
			if(user == null)
				return new ApiResult<GetProfileInfoResponse>(false, ApiResultStatusCode.BadRequest, null, "اطلاعات کاربری یافت نشد!");
			return new ApiResult<GetProfileInfoResponse>(true,ApiResultStatusCode.Success, new GetProfileInfoResponse()
			{
				NickName = user.NickName,
				UserImageDocumentId = user.DocumentId
			});
		}
	}

	public class GetProfileInfoResponse
	{
		public string? NickName { get; set; }
		public int? UserImageDocumentId { get; set; }
	}
}
