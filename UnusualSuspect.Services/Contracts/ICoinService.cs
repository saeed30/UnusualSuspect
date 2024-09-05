using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface ICoinService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(BaseCoinPackageEnum coinPackage,
    int userId, bool bySystem = false, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(int coinPackageId,
    int userId, bool bySystem = false, CancellationToken cancellationToken = default);
  Task<bool> GivenTodayAward(int userId, CancellationToken cancellationToken = default);
}