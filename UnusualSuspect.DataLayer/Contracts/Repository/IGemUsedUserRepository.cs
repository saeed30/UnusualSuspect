using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository
{
  public interface IGemUsedUserRepository : IAsyncRepository<GemUsedUser>
  {
    Task<int> GetSumAsync(int userId);
  }
}
