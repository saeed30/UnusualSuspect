using UnusualSuspect.ApiViewModels.Endpoints.Avatar;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Identity;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class AvatarService(IAvatarPackageRepository avatarPackageRepository,
  IAvatarRepository avatarRepository,
  IAvatarPackageUserRepository avatarPackageUserRepository,
  IPackageEntityService packageEntityService,
  IApplicationUserManager applicationUserManager) : IAvatarService
{
  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default)
  {
    List<AvatarPackage> result = await avatarPackageRepository.GetAllActivePublicAsync(cancellationToken);
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = result.OrderBy(x => x.ViewOrder).ToPackageDto()
      }
    );
  }

  public async Task<UnusualSuspectServiceResult<bool>> BuyPackagesAsync(AvatarPurchaseRequest avatarPurchaseRequest, int userId, CancellationToken cancellationToken = default)
  {
    short? packageId = avatarPurchaseRequest.AvatarPackageId.ToShort();
    if (!packageId.HasValue)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidAvatarPackageId));
    AvatarPackage? package = await avatarPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidAvatarPackageId));
    if (await avatarPackageUserRepository.OwnedByUserAsync(packageId.Value, userId, cancellationToken))
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.AlreadyOwnsThePackage));
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
    var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
    if (!hasEnough.Success)
      return hasEnough;
    if (!hasEnough.Result)
      return new UnusualSuspectServiceResult<bool>(
        new UnusualSuspectErrorResult(LogicErrorCode.DoNotHaveEnoughToPay));
    avatarPackageUserRepository.Add(new AvatarPackageUser()
    {
      UserId = userId,
      AvatarPackageId = packageId.Value,
      TimeAdded = DateTime.Now
    });
    return new UnusualSuspectServiceResult<bool>(true);
  }

}