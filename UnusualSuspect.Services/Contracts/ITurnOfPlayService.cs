
using UnusualSuspect.ApiViewModels.Endpoints.Game;

namespace UnusualSuspect.Services.Contracts;

public interface ITurnOfPlayService
{
  Task<UnusualSuspectServiceResult<bool>> StartTurnOfPlayAsync(int gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int gameId, short orderOfParticipation, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(int userId, CancellationToken cancellationToken = default);
  Task ChangedCandidateCard(int userId, short? cardId, int gameId);
}