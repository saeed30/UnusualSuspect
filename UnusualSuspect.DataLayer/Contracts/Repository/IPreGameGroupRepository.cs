using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IPreGameGroupRepository : IAsyncRepository<PreGameGroup>
{
	Task<IReadOnlyList<PreGameGroup>> GetByUserIdWithJoinedPreGameAsync(int userId, int maxNumberOfGameRequests = 50, CancellationToken cancellationToken = default);
	Task<PreGameGroup?> GetByIdWithJoinedPreGameAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	Task<List<PreGameGroup>> GetTopPreGameGroupByReadyTimeAsync(GameType gameType, int count, CancellationToken cancellationToken = default);
  IQueryable<PreGameGroup> GetAllPreGameGroupsWithDetailsWaitingForGame();
}