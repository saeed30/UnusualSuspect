using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Entities.Dtos;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.ViewModels.Game;

namespace UnusualSuspect.Services.Contracts;

public interface IGameService
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<Game>> GetCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<Game>> GetCurrentGameWithDetailsAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<GameGetResponse>> GetGameResponseAsync(int userId, int? gameId = null, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> LeaveCurrentGameAsync(int userId, CancellationToken cancellationToken = default);
  void LeaveCurrentGame(int userId);
  Task<UnusualSuspectServiceResult<bool>> LeaveGameAsync(int gameId, int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool?>> ChooseCardAndGetWinCondition(int gameId, int characterCardId, int userId, CancellationToken cancellationToken = default);
  IQueryable<Game> GetAllActiveGamesWithGameType();
  Task<bool> StartGameIfAllUsersOnline(int gameId, List<int> userIds);
  Task<bool> GoToTalkingStatus(int gameId);
  Task<bool> GoToTalkingStatus(Game game);
  Task<UnusualSuspectServiceResult<GameDetailsViewModel>> GetDetailByIdAsync(int gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> SetWitnessAnswer(int gameId, bool witnessAnswer, short questionId, int? userId, CancellationToken cancellationToken = default);
  Task<bool> IsGameParticipantAsync(int userId, int gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<GameStatisticsDto>> GetGameStatisticsAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<FinishedResponse>> GetFinishedResponseAsync(int userId, int? gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<TurnOfPlayTalkingState>> StartTurnOfPlayAsync(int gameId);
  Task<UnusualSuspectServiceResult<bool>> UserTurnFinishedAsync(int userId, int gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<TurnOfPlayGetResponse>> GetTurnOfPlayGetAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> ChangedCandidateCard(int userId, short? characterCardId, int gameId);

}