
using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.Services.Contracts;

public interface ITurnOfPlayService
{
  Task<UnusualSuspectServiceResult<TurnOfPlayTalkingState>> StartTurnOfPlayAsync(int gameId);
  Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int userId, int gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> ChangedCandidateCard(int userId, short? characterCardId, int gameId);
}