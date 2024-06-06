using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGemPackageUserRepository : IAsyncRepository<GemPackageUser>
{
  Task<int> GetSumAsync(int userId);
  Task<bool> OwnedByUserAsync(short packageId, int userId, CancellationToken cancellationToken = default);
}