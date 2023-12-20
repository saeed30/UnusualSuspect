using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts;

public interface IMemoryCacheService
{
  Task<List<string>> GetUserSignalRConnections(int userId);
  void SetUserSignalRConnections(int userId, List<string> connections);
  Task<List<string>> GetUserSignalRGroups(int userId);
  void SetUserSignalRGroups(int userId, List<string> groups);
  Task<TurnOfPlayGetResponse?> GetTurnOfPlay(int gameId);
  void SetTurnOfPlay(int gameId, TurnOfPlayGetResponse model);
  Task<Game?> GetGameWithDetails(int gameId, CancellationToken cancellationToken = default);
  void SetGameWithDetails(Game game);
  void ClearGameWithDetails(int gameId);
}