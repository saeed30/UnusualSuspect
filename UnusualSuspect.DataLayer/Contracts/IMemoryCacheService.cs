using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts;

public interface IMemoryCacheService
{
  Task<List<string>> GetUserSignalRConnections(int userId);
  void SetUserSignalRConnections(int userId, List<string> connections);
  Task<List<string>> GetUserSignalRGroups(int userId);
  void SetUserSignalRGroups(int userId, List<string> groups);
  //Task<TurnOfPlayGetResponse?> GetTurnOfPlay(int gameId);
  //void SetTurnOfPlay(int gameId, TurnOfPlayGetResponse model);
  Task ResetTurnOfPlay(int gameId, TurnOfPlayTalkingState model, DateTime currentUserTurnStartedTime, CancellationToken cancellationToken = default);
  Task ResetGameCandidates(int gameId, List<GameCandidate> model, CancellationToken cancellationToken = default);
	Task<Game?> GetGameWithDetails(int gameId, CancellationToken cancellationToken = default);
  void SetGameWithDetails(Game game);
  void ClearGameWithDetails(int gameId);
  Task<List<int>> GetSignalRGroupOnlineUsers(string groupName);
  void SetSignalRGroupOnlineUsers(string groupName, List<int> userIds);
}