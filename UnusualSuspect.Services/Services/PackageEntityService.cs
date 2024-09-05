using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.Common.Utilities;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Services.Services;

public sealed class PackageEntityService(ILogger<PackageEntityService> logger,
  IApplicationUserManager applicationUserManager,
  IGemUsedUserRepository gemUsedUserRepository,
  ICoinUsedUserRepository coinUsedUserRepository,
  IPaymentUserRepository paymentUserRepository) : IPackageEntityService
{
  public UnusualSuspectServiceResult<bool> PayIfHasEnough(PackageEntity package, ApplicationUser user, CancellationToken cancellationToken = default)
  {
    if (!package.PriceTypeId.HasValue || package.Price <= 0)
      return new UnusualSuspectServiceResult<bool>(true);
    bool result;
    switch ((PriceTypeEnum)package.PriceTypeId.Value)
    {
      case PriceTypeEnum.Money:
        if (package is GemPackage)
          return new UnusualSuspectServiceResult<bool>(true);
        logger.LogEvent(SystemEventType.PayPackageWithMoneyShouldNotBeChecked, user.Id, package.Id.ToString(), logLevel: LogLevel.Critical);
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.MoneyOnlyUsedForGem));
      case PriceTypeEnum.Gem:
        result = user.CalculatedGems >= package.Price;
        if (result)
          user.CalculatedGems -= package.Price;
        break;
      case PriceTypeEnum.Coin:
        result = user.CalculatedCoins >= package.Price;
        if (result)
          user.CalculatedCoins -= package.Price;
        break;
      case PriceTypeEnum.Game:
      case PriceTypeEnum.PreGame:
      case PriceTypeEnum.Avatar:
      case PriceTypeEnum.Sticker:
        logger.LogEvent(SystemEventType.PayPackageIsNotValidForPayIfHasEnough, user.Id,
          $"packageId ({package.Id}) - priceType ({package.PriceTypeId.Value})", logLevel: LogLevel.Critical);
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.CanNotUseThisPriceTypeForPayment));
      default:
        throw new ArgumentOutOfRangeException();
    }
    return new UnusualSuspectServiceResult<bool>(result);
  }

  public UnusualSuspectServiceResult<int?> SavePayment(PackageEntity package, int userId, Guid referenceGuid,
    StoreEnum store = StoreEnum.Unknown, string? token = null)
  {
    if (!package.PriceTypeId.HasValue || package.Price <= 0)
      return new UnusualSuspectServiceResult<int?>((int?)null);
    PriceTypeEnum priceTypeUsedFor = GetUsedForPriceType(package);

    switch ((PriceTypeEnum)package.PriceTypeId.Value)
    {
      case PriceTypeEnum.Money:
        if (token == null || !token.HasValue())
          throw new Exception($"PurchaseToken not found. userId: {userId} - packageId: {package.Id}");
        paymentUserRepository.Add(new PaymentUser()
        {
          UserId = userId,
          Amount = package.Price,
          UsedForPriceTypeId = (short)priceTypeUsedFor,
          TimeAdded = DateTime.Now,
          ReferenceGuid = referenceGuid,
          PurchaseToken = token,
          StoreId = (short)store
        });
        break;
      case PriceTypeEnum.Gem:
        gemUsedUserRepository.Add(new GemUsedUser()
        {
          UserId = userId,
          Amount = package.Price,
          UsedForPriceTypeId = (short)priceTypeUsedFor,
          TimeAdded = DateTime.Now,
          ReferenceGuid = referenceGuid
        });
        break;
      case PriceTypeEnum.Coin:
        coinUsedUserRepository.Add(new CoinUsedUser()
        {
          UserId = userId,
          Amount = package.Price,
          UsedForPriceTypeId = (short)priceTypeUsedFor,
          TimeAdded = DateTime.Now,
          ReferenceGuid = referenceGuid
        });
        break;
      case PriceTypeEnum.Game:
      case PriceTypeEnum.PreGame:
      case PriceTypeEnum.Avatar:
      case PriceTypeEnum.Sticker:
        logger.LogEvent(SystemEventType.PayPackageIsNotValidForSavePayment, userId,
          $"packageId ({package.Id}) - priceType ({package.PriceTypeId.Value})", logLevel: LogLevel.Critical);
        return LogicErrorCode.CanNotUseThisPriceTypeForPayment;
      default:
        throw new ArgumentOutOfRangeException();
    }
    return new UnusualSuspectServiceResult<int?>(package.Amount);
  }

  private PriceTypeEnum GetUsedForPriceType(PackageEntity package)
  {
    if (package is AvatarPackage)
      return PriceTypeEnum.Avatar;
    if (package is StickerPackage)
      return PriceTypeEnum.Sticker;
    if (package is GemPackage)
      return PriceTypeEnum.Gem;
    if (package is CoinPackage)
      return PriceTypeEnum.Coin;
    throw new ArgumentOutOfRangeException();
  }
}