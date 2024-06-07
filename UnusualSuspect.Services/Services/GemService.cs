using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;
using UnusualSuspect.ViewModels.Settings;

namespace UnusualSuspect.Services.Services;

public sealed class GemService(IGemPackageRepository gemPackageRepository,
  IPackageEntityService packageEntityService,
  IOptionsSnapshot<ProjectSetting> setting,
  IApplicationUserManager applicationUserManager,
  IGemPackageUserRepository gemPackageUserRepository) : IGemService
{
  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default)
  {
    List<GemPackage> result = await gemPackageRepository.GetAllActivePublicAsync(cancellationToken);
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = result.OrderBy(x => x.ViewOrder).ToPackageDto()
      }
    );
  }

  public async Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(
    int gemPackageId, string purchaseToken, int userId, bool bySystem = false,
    CancellationToken cancellationToken = default)
  {
    if (purchaseToken.IsNull())
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.PurchaseTokenIsEmpty));
    short? packageId = gemPackageId.ToShort();
    if (!packageId.HasValue)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidGemPackageId));
    GemPackage? package = await gemPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidGemPackageId));
    if (!package.IsActive)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(new UnusualSuspectErrorResult(LogicErrorCode.GemPackageIsNotActive));
    if (!bySystem && package.Id < 1000 && package.IsPublic)//one time use package
    {
      if (await gemPackageUserRepository.OwnedByUserAsync(packageId.Value, userId, cancellationToken))
        return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
          new UnusualSuspectErrorResult(LogicErrorCode.AlreadyOwnsThePackage));
    }
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidUserId));
    if (!bySystem)
    {
      var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
      if (!hasEnough.Success)
        return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((false, (PriceTypeEnum?)package.PriceTypeId));
      if (!hasEnough.Result)
        return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
          new UnusualSuspectErrorResult(LogicErrorCode.DoNotHaveEnoughToPay));
    }
    Guid guid = Guid.NewGuid();
    gemPackageUserRepository.Add(new GemPackageUser()
    {
      UserId = userId,
      GemPackageId = packageId.Value,
      TimeAdded = DateTime.Now,
      Guid = guid,
      IsActive = setting.Value.IsTesting, // become true after validation
      Amount = package.Amount
    });
    UnusualSuspectServiceResult<bool> saved = packageEntityService.SavePayment(package, userId, guid, purchaseToken);
    if (!saved.Success)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((false, (PriceTypeEnum?)package.PriceTypeId));
    return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((saved.Result, (PriceTypeEnum?)package.PriceTypeId));
  }
}