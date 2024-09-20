using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.ViewModels.Dto.Gem;

namespace UnusualSuspect.Services.Contracts;

public interface IPackageEntityService
{
  UnusualSuspectServiceResult<bool> PayIfHasEnough(PackageEntity package, ApplicationUser user, CancellationToken cancellationToken = default);
  UnusualSuspectServiceResult<int?> SavePayment(PackageEntity package, int userId, Guid referenceGuid,
    GemPurchaseRequestDto? gemPurchaseRequestDto = null);
}