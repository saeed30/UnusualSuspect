using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGameRepository : IAsyncRepository<Game>
{
	Task<Game?> GetGameWithDetailsAsync(int gameId, CancellationToken cancellationToken = default);
	Task<bool> SetGameStatusAsync(int gameId, GameStatusEnum gameStatus, CancellationToken cancellationToken = default);
  IQueryable<Game> GetAllActiveGamesWithGameType();
  Task<Game?> GetUserCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default);
  Task<Game?> GetUserCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  Task<bool> UserIsInActiveGameAsync(int userId, CancellationToken cancellationToken = default);
}