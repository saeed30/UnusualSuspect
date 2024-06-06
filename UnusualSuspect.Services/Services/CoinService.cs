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

public sealed class CoinService(ICoinPackageRepository coinPackageRepository,
  IApplicationUserManager applicationUserManager,
  IPackageEntityService packageEntityService,
  ICoinPackageUserRepository coinPackageUserRepository) : ICoinService
{
  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default)
  {
    List<CoinPackage> result = await coinPackageRepository.GetAllActivePublicAsync(cancellationToken);
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = result.OrderBy(x => x.ViewOrder).ToPackageDto()
      }
    );
  }

  public async Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(int coinPackageId, int userId, CancellationToken cancellationToken = default)
  {
    short? packageId = coinPackageId.ToShort();
    if (!packageId.HasValue)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidCoinPackageId));
    CoinPackage? package = await coinPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidCoinPackageId));
    if(!package.IsActive)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
        new UnusualSuspectErrorResult(LogicErrorCode.InvalidCoinPackageId));
    if (!package.IsActive)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(new UnusualSuspectErrorResult(LogicErrorCode.CoinPackageIsNotActive));
    if (package.Id < 1000 && package.IsPublic)//one time use package
    {
      if (await coinPackageUserRepository.OwnedByUserAsync(packageId.Value, userId, cancellationToken))
        return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>(
          new UnusualSuspectErrorResult(LogicErrorCode.AlreadyOwnsThePackage));
    }
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
    coinPackageUserRepository.Add(new CoinPackageUser()
    {
      UserId = userId,
      CoinPackageId = packageId.Value,
      TimeAdded = DateTime.Now,
      Guid = guid,
      Amount = package.Amount
    });
    UnusualSuspectServiceResult<bool> saved = packageEntityService.SavePayment(package, userId, guid);
    if (!saved.Success)
      return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((false, (PriceTypeEnum?)package.PriceTypeId));
    return new UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>((saved.Result, (PriceTypeEnum?)package.PriceTypeId));
  }
}