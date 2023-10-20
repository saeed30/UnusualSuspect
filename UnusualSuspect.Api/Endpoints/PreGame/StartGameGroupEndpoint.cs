using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.Api.Endpoints.Account;
using UnusualSuspect.Api.Endpoints.User;
using UnusualSuspect.Common;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Api.Endpoints.PreGame
{
	public class StartGameGroupEndpoint : EndpointBaseAsync
	.WithRequest<short>
	.WithActionResult<ApiResult<StartGameGroupResponse>>
	{
		private readonly IGameService gameService;
		private readonly IApplicationUserManager iApplicationUserManager;
		public StartGameGroupEndpoint(IGameService gameService, IApplicationUserManager iApplicationUserManager)
		{
			this.gameService = gameService;
			this.iApplicationUserManager = iApplicationUserManager;
		}
		public async override Task<ActionResult<ApiResult<StartGameGroupResponse>>> HandleAsync(short id, CancellationToken cancellationToken = default)
		{
			var user = await iApplicationUserManager.FindByNameAsync(HttpContext.User.Identity.Name);
			if(user == null)
				return new ApiResult<StartGameGroupResponse>(false,ApiResultStatusCode.NotFound,null , "کاربر جاری یافت نشد");
			var result = await gameService.StartPreGameGroup(user.Id, id);
			return new ApiResult<StartGameGroupResponse>(true, ApiResultStatusCode.Success, new StartGameGroupResponse()
			{
				GameGroupId = result.Id
			});
		}
	}
	public class StartGameGroupResponse
	{
		public int GameGroupId { get; set; }
	}
}
