using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IGemService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(int gemPackageId,
    string purchaseToken, int userId, bool bySystem = false, CancellationToken cancellationToken = default);
}