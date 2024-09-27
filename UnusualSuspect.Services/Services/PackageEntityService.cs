using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.DataLayer.Repositories;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.ViewModels.Dto.Gem;

namespace UnusualSuspect.Services.Services;

public sealed class PackageEntityService(ILogger<PackageEntityService> logger,
  IApplicationUserManager applicationUserManager,
  IPaymentCafeBazaarRepository paymentCafeBazaarRepository,
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
    GemPurchaseRequestDto? gemPurchaseRequestDto = null)
  {
    if (!package.PriceTypeId.HasValue || package.Price <= 0)
      return new UnusualSuspectServiceResult<int?>((int?)null);
    PriceTypeEnum priceTypeUsedFor = GetUsedForPriceType(package);

    switch ((PriceTypeEnum)package.PriceTypeId.Value)
    {
      case PriceTypeEnum.Money:
        PaymentUser paymentUser = new PaymentUser()
        {
          UserId = userId,
          Amount = package.Price,
          UsedForPriceTypeId = (short)priceTypeUsedFor,
          TimeAdded = DateTime.Now,
          ReferenceGuid = referenceGuid,
          StoreId = (short)StoreEnum.Unknown
        };
        if (gemPurchaseRequestDto != null && gemPurchaseRequestDto.Store == StoreEnum.Cafebazaar)
        {
          if (gemPurchaseRequestDto.CafeBazaarRequestDto == null || string.IsNullOrWhiteSpace(gemPurchaseRequestDto.CafeBazaarRequestDto.PurchaseToken))
            return LogicErrorCode.PurchaseTokenIsEmpty;
          if (string.IsNullOrWhiteSpace(gemPurchaseRequestDto.CafeBazaarRequestDto.ProductId))
            return LogicErrorCode.ProductIdIsEmpty;
          bool alreadyExists = paymentCafeBazaarRepository.GetAll().Any(x =>
            x.PurchaseToken == gemPurchaseRequestDto.CafeBazaarRequestDto.PurchaseToken);
          if(alreadyExists)
            return LogicErrorCode.CafeBazaarPurchaseTokenWasUsed;
          paymentCafeBazaarRepository.Add(new PaymentCafeBazaar()
          {
            IsValid = null,
            ProductId = gemPurchaseRequestDto.CafeBazaarRequestDto.ProductId,
            PaymentUser = paymentUser,
            PurchaseToken = gemPurchaseRequestDto.CafeBazaarRequestDto.PurchaseToken,
            ValidationCheckDateTime = null,
            ValidationError = null
          });
          paymentUser.StoreId = (short)StoreEnum.Cafebazaar;
        }
        paymentUserRepository.Add(paymentUser);
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