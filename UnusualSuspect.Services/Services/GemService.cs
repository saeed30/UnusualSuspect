using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;
using UnusualSuspect.ViewModels.Dto;
using UnusualSuspect.ViewModels.Dto.Gem;
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
    BaseGemPackageEnum gemPackage, int userId, bool isBySystem,
    CancellationToken cancellationToken = default)
  {
    return await BuyPackagesAsync(new GemPurchaseRequestDto()
    {
      GemPackageId = (short)gemPackage,
      Store = StoreEnum.Unknown,
      CafeBazaarRequestDto = null,
      MyketRequestDto = null,
      IsBySystem = isBySystem
    }, userId, cancellationToken);
  }
  public async Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(
    GemPurchaseRequestDto gemPurchaseRequestDto, int userId,
    CancellationToken cancellationToken = default)
  {
    if (gemPurchaseRequestDto.IsBySystem &&
        (gemPurchaseRequestDto.CafeBazaarRequestDto != null || gemPurchaseRequestDto.MyketRequestDto != null || gemPurchaseRequestDto.Store != StoreEnum.Unknown))
      return LogicErrorCode.InvalidParameters;
    GemPackage? package = await gemPackageRepository.GetByIdAsync(gemPurchaseRequestDto.GemPackageId, cancellationToken);
    if (package == null)
      return LogicErrorCode.InvalidGemPackageId;
    if (!package.IsActive)
      return LogicErrorCode.GemPackageIsNotActive;
    if (package.RepetitionTypeId != (short)RepetitionTypeEnum.NoLimit)//one time use package
    {
      DateTime? fromTime;
      switch ((RepetitionTypeEnum)package.RepetitionTypeId)
      {
        case RepetitionTypeEnum.None:
          fromTime = null;
          break;
        case RepetitionTypeEnum.Daily:
          fromTime = DateTime.Now.Date;
          break;
        case RepetitionTypeEnum.Weekly:
          fromTime = DateTime.Now.AddDays(-7);
          break;
        case RepetitionTypeEnum.Monthly:
          fromTime = DateTime.Now.AddMonths(-1);
          break;
        case RepetitionTypeEnum.Yearly:
          fromTime = DateTime.Now.AddYears(-1);
          break;
        case RepetitionTypeEnum.NoLimit:
        default:
          throw new ArgumentOutOfRangeException();
      }
      if (await gemPackageUserRepository.Search(
            new GemPackageUserSearchFilterDto(userId, gemPurchaseRequestDto.GemPackageId, fromTime, null))
            .AnyAsync(cancellationToken))
        return LogicErrorCode.AlreadyOwnsThePackage;
    }
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;
    if (package.Price > 0)
    {
      var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
      if (!hasEnough.Success)
        return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(hasEnough.Errors);
      if (!hasEnough.Result)
        return LogicErrorCode.DoNotHaveEnoughToPay;
    }
    Guid guid = Guid.NewGuid();
    var gemPackageUser = new GemPackageUser()
    {
      UserId = userId,
      GemPackageId = gemPurchaseRequestDto.GemPackageId,
      TimeAdded = DateTime.Now,
      Guid = guid,
      IsActive = gemPurchaseRequestDto.IsBySystem, // become true after validation
      Amount = package.Amount
    };
    gemPackageUserRepository.Add(gemPackageUser);
    if (gemPackageUser.IsActive)
      user.CalculatedGems += package.Amount;
    UnusualSuspectServiceResult<int?> saved = packageEntityService.SavePayment(package, userId, guid, gemPurchaseRequestDto);
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
        DateTime.Now.Date, null))
      .AnyAsync(cancellationToken);
  }
}