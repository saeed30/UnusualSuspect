using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Contracts;

public interface IPackageEntityService
{
  UnusualSuspectServiceResult<bool> PayIfHasEnough(PackageEntity package, ApplicationUser user, CancellationToken cancellationToken = default);
  UnusualSuspectServiceResult<bool> SavePayment(PackageEntity package, int userId, Guid referenceGuid, string? token = null);
}