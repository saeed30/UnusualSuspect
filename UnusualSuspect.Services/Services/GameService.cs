using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services
{
	public class GameService : IGameService
	{
		private readonly IUnitOfWork uow;
		private readonly IGameRepository gameRepository;
		public GameService(IUnitOfWork uow, IGameRepository gameRepository)
		{
			this.uow = uow;
			this.gameRepository = gameRepository;
		}
		public async Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameAsync(int gameId)
		{
			Game? game = await gameRepository.GetByIdAsync(gameId);
			if (game == null)
				return new UnusualSuspectServiceResult<GameGetResponse>(
					new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
			return null;
		}
		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await uow.SaveChangesAsync(cancellationToken);
		}

	}
}
