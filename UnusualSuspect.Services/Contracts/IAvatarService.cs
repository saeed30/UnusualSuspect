using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IAvatarService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(int avatarPackageId, int userId, CancellationToken cancellationToken = default);
}