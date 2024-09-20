using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.ViewModels.Dto.Gem;

namespace UnusualSuspect.Services.Contracts;

public interface IGemService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(BaseGemPackageEnum gemPackage,
    int userId, bool isBySystem, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(GemPurchaseRequestDto gemPurchaseRequestDto, int userId, CancellationToken cancellationToken = default);
  Task<bool> GivenTodayAward(int userId, CancellationToken cancellationToken = default);
}