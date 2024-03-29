using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Common;

public sealed class MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger) : IMemoryCacheService
{
  private const int CacheTimeInMinutes = 60;

  #region UserSignalRConnections
  public async Task<List<string>> GetUserSignalRConnections(int userId)
  {
    var connectionIds = await cache.GetOrCreateAsync(GetUserSignalRConnectionsKey(userId), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult(new List<string>());
    });
    return connectionIds;
  }

  public void SetUserSignalRConnections(int userId, List<string> connections)
  {
    cache.Set(GetUserSignalRConnectionsKey(userId), connections, new MemoryCacheEntryOptions()
    {
      Priority = CacheItemPriority.High,
      SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes),
      Size = 1
    });
  }
  private string GetUserSignalRConnectionsKey(int userId)
  {
    return userId + "UserSignalRConnections";
  }
  #endregion UserSignalRConnections

  #region UserSignalRGroups
  public async Task<List<string>> GetUserSignalRGroups(int userId)
  {
    var groups = await cache.GetOrCreateAsync(GetUserSignalRGroupsKey(userId), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult(new List<string>());
    });
    return groups;
  }

  public void SetUserSignalRGroups(int userId, List<string> groups)
  {
    cache.Set(GetUserSignalRGroupsKey(userId), groups, new MemoryCacheEntryOptions()
    {
      Priority = CacheItemPriority.High,
      SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes),
      Size = 1
    });
  }
  private string GetUserSignalRGroupsKey(int userId)
  {
    return userId + "UserSignalRGroups";
  }

  #endregion UserSignalRGroups
  #region SignalRGroupsOnlineUsers
  public async Task<List<int>> GetSignalRGroupOnlineUsers(string groupName)
  {
    var userIds = await cache.GetOrCreateAsync(GetSignalRGroupOnlineUsersKey(groupName), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult(new List<int>());
    });
    return userIds;
  }

  public void SetSignalRGroupOnlineUsers(string groupName, List<int> userIds)
  {
    cache.Set(GetSignalRGroupOnlineUsersKey(groupName), userIds, new MemoryCacheEntryOptions()
    {
      Priority = CacheItemPriority.High,
      SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes),
      Size = 1
    });
  }
  private string GetSignalRGroupOnlineUsersKey(string groupName)
  {
    return groupName + "UserSignalRGroups";
  }

  #endregion UserSignalRGroups

  //#region TurnOfPlay
  //public async Task<TurnOfPlayGetResponse?> GetTurnOfPlay(int gameId)
  //{
  //  var groups = await cache.GetOrCreateAsync(GetTurnOfPlayKey(gameId), entry =>
  //  {
  //    // Set the cache options for the entry
  //    entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
  //    entry.Priority = CacheItemPriority.High;
  //    entry.Size = 1;

  //    return Task.FromResult((TurnOfPlayGetResponse?)null);
  //  });
  //  return groups;
  //}
  //public void SetTurnOfPlay(int gameId, TurnOfPlayGetResponse model)
  //{
  //  cache.Set(GetTurnOfPlayKey(gameId), model, new MemoryCacheEntryOptions()
  //  {
  //    Priority = CacheItemPriority.High,
  //    SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes),
  //    Size = 1
  //  });
  //}
  //private string GetTurnOfPlayKey(int gameId)
  //{
  //  return gameId + "TurnOfPlay";
  //}
  //#endregion TurnOfPlay
  public async Task ResetTurnOfPlay(int gameId, TurnOfPlayTalkingState model, CancellationToken cancellationToken = default)
  {
    Game? game = await GetGameWithDetails(gameId, cancellationToken);
    if (game == null)
      return;
    game.TalkingTurnStartedTime = model.TalkingTurnStartedTime;
    game.CurrentUserTurnStartedTime = model.CurrentUserTurnStartedTime;
    game.OrderOfParticipationTalkBeginner = model.OrderOfParticipationTalkBeginner;
    game.OrderOfParticipationTurnToTalk = model.OrderOfParticipationTurnToTalk;

    SetGameWithDetails(game);
  }

  public async Task ResetGameCandidates(int gameId, List<GameCandidate> model,
    CancellationToken cancellationToken = default)
  {
    Game? game = await GetGameWithDetails(gameId, cancellationToken);
    if (game == null)
      return;
    game.GameCandidates = model;
    SetGameWithDetails(game);
  }


  #region GameWithDetails
  public void SetGameWithDetails(Game model)
  {
    cache.Set(GetGameWithDetailsKey(model.Id), model, new MemoryCacheEntryOptions()
    {
      Priority = CacheItemPriority.High,
      SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes),
      Size = 1
    });
  }

  public async Task<Game?> GetGameWithDetails(int gameId, CancellationToken cancellationToken = default)
  {
    var groups = await cache.GetOrCreateAsync(GetGameWithDetailsKey(gameId), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult((Game?)null);
    });
    return groups;
  }

  public void ClearGameWithDetails(int gameId)
  {
    cache.Remove(GetGameWithDetailsKey(gameId));
  }

  private string GetGameWithDetailsKey(int gameId)
  {
    return gameId + "GameWithDetails";
  }
  #endregion GameWithDetails
}