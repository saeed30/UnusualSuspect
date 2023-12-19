using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGameRepository : IAsyncRepository<Game>
{
	Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default);
	Task<bool> SetGameWinStateAsync(int id, bool won, CancellationToken cancellationToken = default);
  IQueryable<Game> GetAllActiveGamesWithGameType();
  Task<Game?> GetUserCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default);
  Task<Game?> GetUserCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  Task<bool> UserIsInActiveGameAsync(int userId, CancellationToken cancellationToken = default);
}