using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Contracts;

public interface ICoinUsedService
{
  public UnusualSuspectServiceResult<bool> PayIfHasEnough(int price, ApplicationUser user, PriceTypeEnum priceType,
    Guid referenceGuid, CancellationToken cancellationToken = default);

  Task<bool> DeletePaymentIfExistsAsync(ApplicationUser user, Guid referenceGuid, PriceTypeEnum priceType, CancellationToken cancellationToken = default);
  //bool DeletePaymentIfExists(ApplicationUser user, Guid referenceGuid, PriceTypeEnum priceType);
}