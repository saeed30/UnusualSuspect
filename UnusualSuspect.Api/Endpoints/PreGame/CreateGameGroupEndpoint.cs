using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Api.Endpoints.PreGame;

public sealed class CreateGameGroupEndpoint : EndpointBaseAsync
.WithRequest<CreateGameGroupRequest>
.WithActionResult<ApiResult<CreateGameGroupResponse>>
{
	private readonly IGameService gameService;
	private readonly IApplicationUserManager iApplicationUserManager;
	public CreateGameGroupEndpoint(IGameService gameService, IApplicationUserManager iApplicationUserManager)
	{
		this.gameService = gameService;
		this.iApplicationUserManager = iApplicationUserManager;
	}
	[HttpPost("api/[namespace]/CreateGameGroup")]
	public async override Task<ActionResult<ApiResult<CreateGameGroupResponse>>> HandleAsync([FromBody] CreateGameGroupRequest id, CancellationToken cancellationToken = default)
	{
		var user = await iApplicationUserManager.FindByNameAsync(HttpContext.User.Identity.Name);
		if (user == null)
			return new ApiResult<CreateGameGroupResponse>(false, ApiResultStatusCode.NotFound, null, "کاربر جاری یافت نشد");
		var result = await gameService.CreatePreGameGroup(user.Id, id.KeyValue);
		await gameService.SaveChangesAsync();
		return new ApiResult<CreateGameGroupResponse>(true, ApiResultStatusCode.Success, new CreateGameGroupResponse()
		{
			GameGroupId = result.Id
		});
	}
}
