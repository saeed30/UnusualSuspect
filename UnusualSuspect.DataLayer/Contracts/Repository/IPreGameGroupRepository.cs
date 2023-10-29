using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IPreGameGroupRepository : IAsyncRepository<PreGameGroup>
{
	Task<PreGameGroup?> GetByIdWithJoinedPreGameAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	Task<List<PreGameGroup>> GetTopPreGameGroupByReadyTimeAsync(GameType gameType, int count, CancellationToken cancellationToken = default);
}