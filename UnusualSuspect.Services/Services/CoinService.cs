using Microsoft.EntityFrameworkCore;
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
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.Services.Services;

public sealed class CoinService(ICoinPackageRepository coinPackageRepository,
  IApplicationUserManager applicationUserManager,
  IPackageEntityService packageEntityService,
  ICoinPackageUserRepository coinPackageUserRepository) : ICoinService
{
  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default)
  {
    IEnumerable<CoinPackage> result = await coinPackageRepository.GetAllActivePublicAsync(cancellationToken);
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = result.ToPackageDto()
      }
    );
  }

  public async Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(
    BaseCoinPackageEnum coinPackage, int userId, bool bySystem = false, CancellationToken cancellationToken = default)
  {
    return await BuyPackagesAsync((short)coinPackage, userId, bySystem, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(
    int coinPackageId, int userId, bool bySystem = false, CancellationToken cancellationToken = default)
  {
    short? packageId = coinPackageId.ToShort();
    if (!packageId.HasValue)
      return LogicErrorCode.InvalidCoinPackageId;
    CoinPackage? package = await coinPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return LogicErrorCode.InvalidCoinPackageId;
    if (!package.IsActive)
      return LogicErrorCode.InvalidCoinPackageId;
    if (!package.IsActive)
      return LogicErrorCode.CoinPackageIsNotActive;
    if (package.Id < 1000 && package.IsPublic)//one time use package
    {
      if (await coinPackageUserRepository.Search(
            new CoinPackageUserSearchFilterDto(userId, packageId.Value))
            .AnyAsync(cancellationToken))
        return LogicErrorCode.AlreadyOwnsThePackage;
    }
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;
    var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
    if (!hasEnough.Success)
      return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(hasEnough.Errors);
    if (!hasEnough.Result)
      return LogicErrorCode.DoNotHaveEnoughToPay;
    Guid guid = Guid.NewGuid();
    CoinPackageUser coinPackageUser = new CoinPackageUser()
    {
      UserId = userId,
      CoinPackageId = packageId.Value,
      TimeAdded = DateTime.Now,
      Guid = guid,
      Amount = package.Amount,
      IsActive = bySystem || (!package.PriceTypeId.HasValue || (PriceTypeEnum)package.PriceTypeId.Value != PriceTypeEnum.Money ||
                 package.Price <= 0)
    };
    coinPackageUserRepository.Add(coinPackageUser);
    if (coinPackageUser.IsActive)
      user.CalculatedCoins += package.Amount;
    UnusualSuspectServiceResult<int?> saved = packageEntityService.SavePayment(package, userId, guid);
    if (!saved.Success)
      return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(saved.Errors);
    return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>((package.Amount, (PriceTypeEnum?)package.PriceTypeId));
  }

  public async Task<bool> GivenTodayAward(int userId, CancellationToken cancellationToken = default)
  {
    return await coinPackageUserRepository
      .Search(new CoinPackageUserSearchFilterDto(
        userId,
        (short)BaseCoinPackageEnum.DailyAward,
        true))
      .AnyAsync(cancellationToken);

  }
}