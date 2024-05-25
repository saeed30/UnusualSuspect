using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;
public interface ICoinPackageRepository : IAsyncRepository<CoinPackage, short>
{
  Task<List<CoinPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken);
}
