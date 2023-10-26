using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ApiViewModels.Enums;

namespace UnusualSuspect.Api.Endpoints.PreGame
{
	public class AddUserToPreGameEndpoint : MyBaseEndpointAuthenticated
		.WithRequest<AddUserToPreGameRequest>
		.WithActionResult<ApiResult>
	{
		private readonly IGameService gameService;
		public AddUserToPreGameEndpoint(IGameService gameService)
		{
			this.gameService = gameService;
		}
		[HttpPost("api/[namespace]/AddUserToPreGame")]
		public override async Task<ActionResult<ApiResult>> HandleAsync(AddUserToPreGameRequest request, CancellationToken cancellationToken = default)
		{
			var result = await gameService.AddUserToPreGameGroup(CurrentUser.UserId, request.UserPhoneNumber, request.PreGameGroupId);
			if (!result.Success)
				return new ApiResult(false, ApiResultStatusCode.LogicError, result.MainError.ToString());
			await gameService.SaveChangesAsync(cancellationToken);
			return new ApiResult(true, ApiResultStatusCode.Success);
		}
	}
}
