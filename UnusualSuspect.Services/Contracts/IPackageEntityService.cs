using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;

namespace UnusualSuspect.Services.Contracts
{
  public interface IPackageEntityService
  {
    UnusualSuspectServiceResult<bool> PayIfHasEnough(PackageEntity package, ApplicationUser user, CancellationToken cancellationToken = default);
  }
}
