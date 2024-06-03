using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.Entities.Common;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;

namespace UnusualSuspect.Services.Services
{
  public sealed class PackageEntityService(ILogger<PackageEntityService> logger,
    IApplicationUserManager applicationUserManager) : IPackageEntityService
  {
    public UnusualSuspectServiceResult<bool> PayIfHasEnough(PackageEntity package, ApplicationUser user, CancellationToken cancellationToken = default)
    {
      if (!package.PriceTypeId.HasValue || package.Price <= 0)
        return new UnusualSuspectServiceResult<bool>(true);
      bool result;
      switch ((PriceTypeEnum)package.PriceTypeId.Value)
      {
        case PriceTypeEnum.Money:
          logger.LogCritical("Buying with money should not be checked here! userId: {userId}, packageId: {packageId}", userId, package.Id);
          return new UnusualSuspectServiceResult<bool>(false);
        case PriceTypeEnum.Gem:
          result = user.CalculatedGems >= package.Price;
          if(result)
            user.CalculatedGems -= package.Price;
          break;
        case PriceTypeEnum.Coin:
          result = user.CalculatedCoins >= package.Price;
          if (result)
            user.CalculatedCoins -= package.Price;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      return new UnusualSuspectServiceResult<bool>(result);
    }
  }
}
