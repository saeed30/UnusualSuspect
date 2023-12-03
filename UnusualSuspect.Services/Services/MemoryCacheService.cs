using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Services.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UnusualSuspect.Services.Services
{
  public sealed class MemoryCacheService(IMemoryCache cache) : IMemoryCacheService
  {
    public async Task<List<string>> GetUserSignalRConnections(int userId)
    {
      var connectionIds = await cache.GetOrCreateAsync(GetUserSignalRConnectionsKey(userId), entry =>
      {
        // Set the cache options for the entry
        entry.SlidingExpiration = TimeSpan.FromMinutes(30);
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
        SlidingExpiration = TimeSpan.FromMinutes(30),
        Size = 1
      });
    }


    public async Task<List<string>> GetUserSignalRGroups(int userId)
    {
      var groups = await cache.GetOrCreateAsync(GetUserSignalRGroupsKey(userId), entry =>
      {
        // Set the cache options for the entry
        entry.SlidingExpiration = TimeSpan.FromMinutes(30);
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
        SlidingExpiration = TimeSpan.FromMinutes(30),
        Size = 1
      });
    }
    private string GetUserSignalRConnectionsKey(int userId)
    {
      return userId + "UserSignalRConnections";
    }
    private string GetUserSignalRGroupsKey(int userId)
    {
      return userId + "UserSignalRGroups";
    }
  }
}
