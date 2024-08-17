using System.Threading;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;

namespace UnusualSuspect.Services.Services;

public sealed class CoinUsedService(ICoinUsedUserRepository coinUsedUserRepository) : ICoinUsedService
{
  public UnusualSuspectServiceResult<bool> PayIfHasEnough(int price, ApplicationUser user, PriceTypeEnum priceType,
    Guid referenceGuid, CancellationToken cancellationToken = default)
  {
    if (price <= 0)
      return new UnusualSuspectServiceResult<bool>(true);
    if(user.CalculatedCoins < price)
      return new UnusualSuspectServiceResult<bool>(false);
    user.CalculatedCoins -= price;

    if (priceType != PriceTypeEnum.Game && priceType != PriceTypeEnum.PreGame)
      throw new NotImplementedException("direct SavePayment is just available for game and preGame");
    coinUsedUserRepository.Add(new CoinUsedUser()
    {
      UserId = user.Id,
      Amount = price,
      UsedForPriceTypeId = (short)priceType,
      TimeAdded = DateTime.Now,
      ReferenceGuid = referenceGuid
    });
    return new UnusualSuspectServiceResult<bool>(true);

  }

  //public bool DeletePaymentIfExists(ApplicationUser user, Guid referenceGuid, PriceTypeEnum priceType)
  //{
  //  if (priceType != PriceTypeEnum.PreGame)
  //    throw new NotImplementedException("delete SavePayment is just available for preGame");
  //  var payment =  coinUsedUserRepository.GetPreGameSavePayment(referenceGuid);
  //  if (payment != null)
  //  {
  //    coinUsedUserRepository.Delete(payment);
  //    user.CalculatedCoins += payment.Amount;
  //    return true;
  //  }
  //  return false;
  //}
  public async Task<bool> DeletePaymentIfExistsAsync(ApplicationUser user, Guid referenceGuid, PriceTypeEnum priceType, CancellationToken cancellationToken = default)
  {
    if (priceType != PriceTypeEnum.PreGame)
      throw new NotImplementedException("delete SavePayment is just available for preGame");
    var payment = await coinUsedUserRepository.GetPreGameSavePaymentAsync(referenceGuid, cancellationToken);
    if (payment != null)
    {
      coinUsedUserRepository.Delete(payment);
      user.CalculatedCoins += payment.Amount;
      return true;
    }
    return false;
  }
}