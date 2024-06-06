using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class AvatarService(IAvatarPackageRepository avatarPackageRepository,
  IAvatarRepository avatarRepository,
  IAvatarPackageUserRepository avatarPackageUserRepository,
  IPackageEntityService packageEntityService,
  IApplicationUserManager applicationUserManager) : IAvatarService
{
  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(int userId, CancellationToken cancellationToken = default)
  {
    List<AvatarPackage> result = await avatarPackageRepository.GetAllActivePublicAsync(cancellationToken);
    List<PackageDto> packages = result.OrderBy(x => x.ViewOrder).ToPackageDto();
    List<short> ownedPackageIds = await avatarPackageUserRepository.OwnedByUserAsync(userId, cancellationToken);
    foreach (PackageDto packageDto in packages)
      if (ownedPackageIds.Contains((short)packageDto.Id))
        packageDto.Enabled = false;
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = packages
      }
    );
  }

  public async Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(int avatarPackageId, int userId, CancellationToken cancellationToken = default)
  {
    short? packageId = avatarPackageId.ToShort();
    if (!packageId.HasValue)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidAvatarPackageId));
    AvatarPackage? package = await avatarPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidAvatarPackageId));
    if (await avatarPackageUserRepository.OwnedByUserAsync(packageId.Value, userId, cancellationToken))
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.AlreadyOwnsThePackage));
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
    var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
    if (!hasEnough.Success)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((false, (PriceTypeEnum?)package.PriceTypeId));
    if (!hasEnough.Result)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.DoNotHaveEnoughToPay));
    Guid guid = Guid.NewGuid();
    avatarPackageUserRepository.Add(new AvatarPackageUser()
    {
      UserId = userId,
      AvatarPackageId = packageId.Value,
      TimeAdded = DateTime.Now,
      Guid = guid
    });
    UnusualSuspectServiceResult<bool> saved = packageEntityService.SavePayment(package, userId, guid);
    if (!saved.Success)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((false, (PriceTypeEnum?)package.PriceTypeId));
    return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((saved.Result, (PriceTypeEnum?)package.PriceTypeId));
  }

}