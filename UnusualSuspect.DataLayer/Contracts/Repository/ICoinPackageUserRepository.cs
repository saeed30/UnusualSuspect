using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface ICoinPackageUserRepository : IAsyncRepository<CoinPackageUser>
{
  Task<int> GetSumAsync(int userId);
  Task<bool> OwnedByUserAsync(short packageId, int userId, CancellationToken cancellationToken = default);
}