using UnusualSuspect.Entities.GameModels;

namespace UnusualSuspect.DataLayer.Contracts.Repository;

public interface IStickerPackageRepository : IAsyncRepository<StickerPackage, short>
{
  Task<List<StickerPackage>> GetAllActivePublicAsync(CancellationToken cancellationToken = default);
}