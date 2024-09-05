using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Contracts;

public interface IPackageEntityService
{
  UnusualSuspectServiceResult<bool> PayIfHasEnough(PackageEntity package, ApplicationUser user, CancellationToken cancellationToken = default);
  UnusualSuspectServiceResult<int?> SavePayment(PackageEntity package, int userId, Guid referenceGuid,
    StoreEnum store = StoreEnum.Unknown, string? token = null);
}