using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services
{
  public sealed class MemoryCacheService(IMemoryCache cache) : IMemoryCacheService
  {
    public List<string> GetUserSignalRConnections(int userId)
    {
      throw new NotImplementedException();
    }

    public void SetUserSignalRConnections(int userId, List<string> connections)
    {
      throw new NotImplementedException();
    }

    public List<string> GetUserSignalRGroups(int userId)
    {
      throw new NotImplementedException();
    }

    public void SetUserSignalRGroups(int userId, List<string> groups)
    {
      throw new NotImplementedException();
    }
  }
}
