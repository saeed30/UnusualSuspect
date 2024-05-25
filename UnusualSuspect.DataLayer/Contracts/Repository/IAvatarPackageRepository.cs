using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IAvatarPackageRepository : IAsyncRepository<AvatarPackage, short>
{
  Task<List<AvatarPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken = default);
}