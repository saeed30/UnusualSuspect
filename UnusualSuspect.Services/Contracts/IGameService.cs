using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.Services.Contracts
{
	public interface IGameService
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameAsync(int gameId);
    Task<UnusualSuspectServiceResult<bool>> FinishGameAsync(int gameId, int userId);
  }
}
