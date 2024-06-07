using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface ICoinService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);

  Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(int coinPackageId,
    int userId, bool bySystem = false, CancellationToken cancellationToken = default);
}