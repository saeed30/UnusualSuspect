using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.Services.Contracts
{
	public interface IGameService
	{
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameAsync(int gameId, int userId, CancellationToken cancellationToken = default);
    Task<UnusualSuspectServiceResult<bool>> FinishGameAsync(int gameId, int userId, CancellationToken cancellationToken = default);
    Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default);
  }
}
