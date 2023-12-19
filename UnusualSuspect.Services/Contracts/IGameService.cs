using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.Services.Contracts;

public interface IGameService
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<GameGetResponse?>> GetCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> LeaveCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> LeaveGameAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default);
  IQueryable<Game> GetAllActiveGamesWithGameType();

}