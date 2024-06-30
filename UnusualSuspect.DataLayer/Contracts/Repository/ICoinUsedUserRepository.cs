using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICoinUsedUserRepository : IAsyncRepository<CoinUsedUser>
{
  Task<int> GetSumAsync(int userId);
}