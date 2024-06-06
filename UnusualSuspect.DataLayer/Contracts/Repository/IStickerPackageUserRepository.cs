using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IStickerPackageUserRepository : IAsyncRepository<StickerPackageUser>
{
  Task<bool> OwnedByUserAsync(short packageId, int userId, CancellationToken cancellationToken = default);
  Task<List<short>> OwnedByUserAsync(int userId, CancellationToken cancellationToken = default);
}