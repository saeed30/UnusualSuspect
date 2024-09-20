using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGameRepository : IAsyncRepository<Game>
{
	Task<Game?> GetGameWithDetailsAsync(int gameId, CancellationToken cancellationToken = default, bool ignoreCache = false);
  IQueryable<Game> GetAllActiveGamesWithGameType();
  Task<Game?> GetUserCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default);
  Task<Game?> GetUserCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  Task<bool> UserIsInActiveGameAsync(int userId, CancellationToken cancellationToken = default);
  Task SetNewTurnToTalk(int gameId, short orderOfParticipationTurnToTalk, DateTime currentUserTurnStartedTime, CancellationToken cancellationToken = default);
  Task<GameStatisticsDto?> GetGameStatisticsAsync(int userId, CancellationToken cancellationToken = default);
}