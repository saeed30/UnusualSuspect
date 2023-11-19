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
	public class GameService(IUnitOfWork uow, IGameRepository gameRepository) : IGameService
	{
    public async Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameAsync(int gameId)
		{
			Game? game = await gameRepository.GetByIdAsync(gameId);
			if (game == null)
				return new UnusualSuspectServiceResult<GameGetResponse>(
					new UnusualSuspectErrorResult(LogicErrorCode.InvalidGameId));
      return new UnusualSuspectServiceResult<GameGetResponse>(new GameGetResponse(null, null));
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await uow.SaveChangesAsync(cancellationToken);
		}

	}
}
