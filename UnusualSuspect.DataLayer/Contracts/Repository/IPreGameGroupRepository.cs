using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
	public interface IPreGameGroupRepository : IAsyncRepository<PreGameGroup>
	{
		Task<PreGameGroup?> GetByIdWithJoinedPreGameAsync(int preGameGroupId, CancellationToken cancellationToken = default);
	}
}
