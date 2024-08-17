using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IPreGameGroupRepository : IAsyncRepository<PreGameGroup>
{
	Task<IReadOnlyList<PreGameGroup>> GetByUserIdWithJoinedPreGameAsync(int userId, int maxNumberOfGameRequests = 50, CancellationToken cancellationToken = default);
	Task<PreGameGroup?> GetByIdWithDetailAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	Task<List<PreGameGroup>> GetTopPreGameGroupByReadyTimeAsync(GameType gameType, int count, CancellationToken cancellationToken = default);
  IQueryable<PreGameGroup> GetAllPreGameGroupsWithDetailsWaitingForGame();
  Task<List<int>> ResetGroupsStatusAfterFinishingTheGameAsync(int gameId, CancellationToken cancellationToken = default);
  Task<PreGameGroup?> GetByIdWithGameTypeAsync(int preGameGroupId, CancellationToken cancellationToken = default);
  Task<PreGameGroup?> GetFirstExpiredPregameGroupWithDetailsAsync(int expireMinutes, CancellationToken cancellationToken);
}