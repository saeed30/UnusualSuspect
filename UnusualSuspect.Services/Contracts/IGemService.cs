using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IGemService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(BaseGemPackageEnum gemPackage,
    string purchaseToken, int userId, StoreEnum store, bool bySystem = false, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(int gemPackageId,
    string purchaseToken, int userId, StoreEnum store, bool bySystem = false, CancellationToken cancellationToken = default);
  Task<bool> GivenTodayAward(int userId, CancellationToken cancellationToken = default);
}