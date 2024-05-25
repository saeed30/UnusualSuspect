using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IGemService
{
  Task<UnusualSuspectServiceResult<bool>> SaveUserGemAsync(int userId,
    short gemPackageId, CancellationToken cancellationToken = default);

  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
}