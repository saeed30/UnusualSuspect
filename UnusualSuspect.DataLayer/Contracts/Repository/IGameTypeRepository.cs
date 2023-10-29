using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGameTypeRepository : IAsyncRepository<GameType, short>
{
	Task<List<GameType>> GetActiveGameTypesAsync(CancellationToken cancellationToken = default);
}