using Microsoft.EntityFrameworkCore;
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
using UnusualSuspect.ViewModels.Dto;
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

  public async Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(
    BaseGemPackageEnum gemPackage, string purchaseToken, int userId, StoreEnum store, bool bySystem = false,
    CancellationToken cancellationToken = default)
  {
    return await BuyPackagesAsync((short)gemPackage, purchaseToken, userId, store, bySystem, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(
    int gemPackageId, string purchaseToken, int userId, StoreEnum store, bool bySystem = false,
    CancellationToken cancellationToken = default)
  {
    if (purchaseToken.IsNull())
      return LogicErrorCode.PurchaseTokenIsEmpty;
    short? packageId = gemPackageId.ToShort();
    if (!packageId.HasValue)
      return LogicErrorCode.InvalidGemPackageId;
    GemPackage? package = await gemPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return LogicErrorCode.InvalidGemPackageId;
    if (!package.IsActive)
      return LogicErrorCode.GemPackageIsNotActive;
    if (!bySystem && package.Id < 1000 && package.IsPublic)//one time use package
    {
      if (await gemPackageUserRepository.Search(
            new GemPackageUserSearchFilterDto(userId, packageId.Value))
            .AnyAsync(cancellationToken))
        return LogicErrorCode.AlreadyOwnsThePackage;
    }
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;
    if (!bySystem)
    {
      var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
      if (!hasEnough.Success)
        return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(hasEnough.Errors);
      if (!hasEnough.Result)
        return LogicErrorCode.DoNotHaveEnoughToPay;
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
    UnusualSuspectServiceResult<int?> saved = packageEntityService.SavePayment(package, userId, guid, store, purchaseToken);
    if (!saved.Success)
      return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(saved.Errors);
    return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>((package.Amount, (PriceTypeEnum?)package.PriceTypeId));
  }

  public async Task<bool> GivenTodayAward(int userId, CancellationToken cancellationToken = default)
  {
    return await gemPackageUserRepository
      .Search(new GemPackageUserSearchFilterDto(
        userId,
        (short)BaseGemPackageEnum.DailyAward,
        true))
      .AnyAsync(cancellationToken);
  }
}