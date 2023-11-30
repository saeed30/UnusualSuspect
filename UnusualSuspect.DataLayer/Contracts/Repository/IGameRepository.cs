using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGameRepository : IAsyncRepository<Game>
{
	Task<Game?> GetGameWithDetailsAsync(int id, CancellationToken cancellationToken = default);
	Task<bool> SetGameFinishTimeAsync(int id, DateTime finishTime, CancellationToken cancellationToken = default);
	Task<bool> SetGameWinStateAsync(int id, bool won, CancellationToken cancellationToken = default);
}