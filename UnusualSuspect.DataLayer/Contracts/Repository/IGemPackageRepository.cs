using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IGemPackageRepository : IAsyncRepository<GemPackage, short>
{
  IEnumerable<GemPackage> GetAllActive();
  Task<List<GemPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken);
}