using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Common.Enums;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.ViewModels.Dto;

namespace UnusualSuspect.Services.Services;

public class PaymentUserService(IPaymentUserRepository paymentUserRepository,
  IApiCallService apiCallService,
  IPaymentCafeBazaarRepository cafeBazaarRepository,
  IGemPackageUserRepository gemPackageUserRepository,
  ICoinPackageUserRepository coinPackageUserRepository,
  ILogger<PaymentUserService> logger,
  IUnitOfWork uow) : IPaymentUserService
{
  public async Task CheckAllUncheckedPayments(int? userId = null, CancellationToken cancellationToken = default)
  {
    List<PaymentCafeBazaar> result;
    if (userId.HasValue)
      result = await cafeBazaarRepository.GetAll()
      .Include(x => x.PaymentUser)
      .ThenInclude(x => x.ApplicationUser)
      .Where(x => !x.IsValid.HasValue && x.PaymentUserId == userId.Value).ToListAsync(cancellationToken);
    else
      result = await cafeBazaarRepository.GetAll()
      .Include(x => x.PaymentUser)
      .ThenInclude(x => x.ApplicationUser)
      .Where(x => !x.IsValid.HasValue).Take(100).ToListAsync(cancellationToken);
    //Cafebazaar
    foreach (PaymentCafeBazaar paymentCafeBazaar in result)
    {
      UnusualSuspectServiceResult<(bool, string?)> apiResult = await apiCallService.CheckPaymentInCafebazaar(paymentCafeBazaar, cancellationToken);
      if (!apiResult.Success)
      {
        logger.LogEvent(SystemEventType.CheckPaymentInCafebazaarError, paymentCafeBazaar.PaymentUserId,
          apiResult.MainError.GetDisplay(), logLevel: LogLevel.Error);
        continue;
      }
      paymentCafeBazaar.IsValid = apiResult.Result.Item1;
      paymentCafeBazaar.ValidationError = apiResult.Result.Item2;
      paymentCafeBazaar.ValidationCheckDateTime = DateTime.Now;

      paymentCafeBazaar.PaymentUser.IsValid = apiResult.Result.Item1;
      if (apiResult.Result.Item1)
      {
        switch ((PriceTypeEnum)paymentCafeBazaar.PaymentUser.UsedForPriceTypeId)
        {
          case PriceTypeEnum.Gem:
            var gemPackage = await gemPackageUserRepository
              .GetAll().FirstOrDefaultAsync(x => x.Guid == paymentCafeBazaar.PaymentUser.ReferenceGuid, cancellationToken);
            if (gemPackage == null)
              throw new Exception("Gem package not found");
            gemPackage.IsActive = true;
            paymentCafeBazaar.PaymentUser.ApplicationUser.CalculatedGems += gemPackage.Amount;
            break;
          case PriceTypeEnum.Coin:
            var coinPackage = await coinPackageUserRepository
              .GetAll().FirstOrDefaultAsync(x => x.Guid == paymentCafeBazaar.PaymentUser.ReferenceGuid, cancellationToken);
            if (coinPackage == null)
              throw new Exception("Coin package not found");
            coinPackage.IsActive = true;
            paymentCafeBazaar.PaymentUser.ApplicationUser.CalculatedCoins += coinPackage.Amount;
            break;
          case PriceTypeEnum.Avatar:
          case PriceTypeEnum.Sticker:
          case PriceTypeEnum.Game:
          case PriceTypeEnum.PreGame:
          case PriceTypeEnum.Money:
          default:
            throw new ArgumentOutOfRangeException(paymentCafeBazaar.PaymentUser.UsedForPriceTypeId.ToString());
        }
      }
      await uow.SaveChangesAsync(cancellationToken);
    }
    //Myket
    //foreach (PaymentUser paymentUser in await result.Where(x => x.StoreId == (short)StoreEnum.Myket).ToListAsync(cancellationToken))
    //{
    //  UnusualSuspectServiceResult<(bool, string?)> apiResult = await apiCallService.CheckPaymentInMyket(paymentUser, cancellationToken);
    //  if (!apiResult.Success)
    //    continue;
    //  paymentUser.IsValid = apiResult.Result.Item1;
    //  //paymentUser.ValidationCheckDateTime = DateTime.Now;
    //  //paymentUser.ValidationError = apiResult.Result.Item2;
    //  await uow.SaveChangesAsync(cancellationToken);
    //}
  }
}