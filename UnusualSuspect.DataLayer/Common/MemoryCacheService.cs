using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Endpoints.Game;
using UnusualSuspect.DataLayer.Contracts;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Models;

namespace UnusualSuspect.DataLayer.Common;

public sealed class MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger) : IMemoryCacheService
{
  private const int CacheTimeInMinutes = 60;

  #region UserSignalRConnections
  public async Task<IEnumerable<string>> GetUserSignalRConnections(int userId)
  {
    var connectionIds = await cache.GetOrCreateAsync(GetUserSignalRConnectionsKey(userId), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult(new List<string>().AsEnumerable());
    });
    return connectionIds;
  }

  public void SetUserSignalRConnections(int userId, IEnumerable<string> connections)
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
  public async Task<IEnumerable<string>> GetUserSignalRGroups(int userId)
  {
    var groups = await cache.GetOrCreateAsync(GetUserSignalRGroupsKey(userId), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult(new List<string>().AsEnumerable());
    });
    return groups;
  }

  public void SetUserSignalRGroups(int userId, IEnumerable<string> groups)
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
  public async Task<IEnumerable<int>> GetSignalRGroupOnlineUsers(string groupName)
  {
    var userIds = await cache.GetOrCreateAsync(GetSignalRGroupOnlineUsersKey(groupName), entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult(new List<int>().AsEnumerable());
    });
    return userIds;
  }

  public void SetSignalRGroupOnlineUsers(string groupName, IEnumerable<int> userIds)
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

  public async Task ResetTurnOfPlay(int gameId, TurnOfPlayTalkingState model, DateTime currentUserTurnStartedTime, CancellationToken cancellationToken = default)
  {
    Game? game = await GetGameWithDetails(gameId, cancellationToken);
    if (game == null)
      return;
    game.TalkingTurnStartedTime = DateTime.Parse(model.TalkingTurnStartedTimeString);
    game.CurrentUserTurnStartedTime = currentUserTurnStartedTime;
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
    model.CachedTime = DateTime.Now;
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

  #region SoftSetting

  private readonly string softSettingKey = "SoftSettingKey";
  public void SetGameWithDetails(SoftSetting model)
  {
    cache.Set(softSettingKey, model, new MemoryCacheEntryOptions()
    {
      Priority = CacheItemPriority.High,
      SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes),
      Size = 1
    });
  }

  public SoftSetting? GetSoftSetting()
  {
    SoftSetting? setting = cache.GetOrCreate(softSettingKey, entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return (SoftSetting?)null;
    });
    return setting;
  }
  public async Task<SoftSetting?> GetSoftSettingAsync(CancellationToken cancellationToken = default)
  {
    var setting = await cache.GetOrCreateAsync(softSettingKey, entry =>
    {
      // Set the cache options for the entry
      entry.SlidingExpiration = TimeSpan.FromMinutes(CacheTimeInMinutes);
      entry.Priority = CacheItemPriority.High;
      entry.Size = 1;

      return Task.FromResult((SoftSetting?)null);
    });
    return setting;
  }
  public void ClearSoftSetting()
  {
    cache.Remove(softSettingKey);
  }


  #endregion SoftSetting
}