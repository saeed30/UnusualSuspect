
namespace UnusualSuspect.Services.Contracts;

public interface IMemoryCacheService
{
  Task<List<string>> GetUserSignalRConnections(int userId);
  void SetUserSignalRConnections(int userId, List<string> connections);
  Task<List<string>> GetUserSignalRGroups(int userId);
  void SetUserSignalRGroups(int userId, List<string> groups);
}