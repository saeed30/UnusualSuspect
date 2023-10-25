using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.PreGame;
using UnusualSuspect.Common.Models;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Api.Endpoints.PreGame
{
	public class AddUserToPreGameEndpoint : MyBaseEndpointAuthenticated
		.WithRequest<AddUserToPreGameRequest>
		.WithActionResult<ApiResult>
	{
		private readonly IGameService gameService;
		private readonly IApplicationUserManager iApplicationUserManager;
		public AddUserToPreGameEndpoint(IGameService gameService, IApplicationUserManager iApplicationUserManager)
		{
			this.gameService = gameService;
			this.iApplicationUserManager = iApplicationUserManager;
		}
		[HttpPost("api/[namespace]/AddUserToPreGame")]
		public override async Task<ActionResult<ApiResult>> HandleAsync(AddUserToPreGameRequest request, CancellationToken cancellationToken = default)
		{
			return null;
		}
	}
}
