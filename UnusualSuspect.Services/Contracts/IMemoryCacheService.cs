using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusualSuspect.Services.Contracts
{
  public interface IMemoryCacheService
  {
    List<string> GetUserSignalRConnections(int userId);
    void SetUserSignalRConnections(int userId, List<string> connections);
    List<string> GetUserSignalRGroups(int userId);
    void SetUserSignalRGroups(int userId, List<string> groups);
  }
}
