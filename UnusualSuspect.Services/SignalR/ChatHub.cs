using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.SignalR
{
  using Microsoft.AspNetCore.SignalR;
  using Microsoft.Extensions.Caching.Memory;
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;

  public class ChatHub(IMemoryCache cache) : Hub
  {
    public override async Task OnConnectedAsync()
    {
      // Get the userId from the query string
      var userId = Context.GetHttpContext().Request.Query["userId"];

      // Get or create a list of connectionIds for the userId
      var connectionIds = await cache.GetOrCreateAsync(userId, entry =>
      {
        // Set the cache options for the entry
        entry.SlidingExpiration = TimeSpan.FromMinutes(30);
        entry.Priority = CacheItemPriority.High;
        entry.Size = 1;

        // Return a new list of connectionIds
        return Task.FromResult(new List<string>());
      });

      // Add the current connectionId to the list
      connectionIds.Add(Context.ConnectionId);

      // Set the list back to the cache
      cache.Set(userId, connectionIds);

      await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
      // Get the userId from the query string
      var userId = Context.GetHttpContext().Request.Query["userId"];

      // Get the list of connectionIds for the userId from the cache
      var connectionIds = cache.Get<List<string>>(userId);

      // Remove the current connectionId from the list
      connectionIds.Remove(Context.ConnectionId);

      // Set the list back to the cache
      cache.Set(userId, connectionIds);

      await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(string groupName)
    {
      // Get the userId from the query string
      var userId = Context.GetHttpContext().Request.Query["userId"];

      // Get or create a dictionary of groups for the userId
      var groups = await cache.GetOrCreateAsync(userId + "_groups", entry =>
      {
        // Set the cache options for the entry
        entry.SlidingExpiration = TimeSpan.FromMinutes(30);
        entry.Priority = CacheItemPriority.High;
        entry.Size = 1;

        // Return a new dictionary of groups
        return Task.FromResult(new Dictionary<string, bool>());
      });

      // Add the groupName to the dictionary
      groups[groupName] = true;

      // Set the dictionary back to the cache
      cache.Set(userId + "_groups", groups);

      // Join the SignalR group
      await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
      // Get the userId from the query string
      var userId = Context.GetHttpContext().Request.Query["userId"];

      // Get the dictionary of groups for the userId from the cache
      var groups = cache.Get<Dictionary<string, bool>>(userId + "_groups");

      // Remove the groupName from the dictionary
      groups.Remove(groupName);

      // Set the dictionary back to the cache
      cache.Set(userId + "_groups", groups);

      // Leave the SignalR group
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
  }
}
