using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Contracts;

public interface IMemoryCacheService
{
  Task<IEnumerable<string>> GetUserSignalRConnections(int userId);
  void SetUserSignalRConnections(int userId, IEnumerable<string> connections);
  Task<IEnumerable<string>> GetUserSignalRGroups(int userId);
  void SetUserSignalRGroups(int userId, IEnumerable<string> groups);
  //Task<TurnOfPlayGetResponse?> GetTurnOfPlay(int gameId);
  //void SetTurnOfPlay(int gameId, TurnOfPlayGetResponse model);
  Task ResetTurnOfPlay(int gameId, TurnOfPlayTalkingState model, DateTime currentUserTurnStartedTime, CancellationToken cancellationToken = default);
  Task ResetGameCandidates(int gameId, List<GameCandidate> model, CancellationToken cancellationToken = default);
	Task<Game?> GetGameWithDetails(int gameId, CancellationToken cancellationToken = default);
  void SetGameWithDetails(Game game);
  void ClearGameWithDetails(int gameId);
  Task<IEnumerable<int>> GetSignalRGroupOnlineUsers(string groupName);
  void SetSignalRGroupOnlineUsers(string groupName, IEnumerable<int> userIds);
  void SetGameWithDetails(SoftSetting model);
  Task<SoftSetting?> GetSoftSettingAsync(CancellationToken cancellationToken = default);
  SoftSetting? GetSoftSetting();
  void ClearSoftSetting();
}