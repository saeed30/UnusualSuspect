using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGameRepository : IAsyncRepository<Game>
{
	Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default);
	Task<bool> SetGameFinishTime(int id, DateTime finishTime, CancellationToken cancellationToken = default);
}