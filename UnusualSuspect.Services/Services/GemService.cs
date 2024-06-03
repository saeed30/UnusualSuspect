using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class GemService(IGemPackageRepository gemPackageRepository, IGemPackageUserRepository gemRepository) : IGemService
{
  public async Task<UnusualSuspectServiceResult<bool>> SaveUserGemAsync(int userId, short gemPackageId, CancellationToken cancellationToken = default)
  {
    var package = await gemPackageRepository.GetByIdAsync(gemPackageId, cancellationToken);
    if (package == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidGemPackageId));
    if (!package.IsActive)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.GemPackageIsInActive));
    gemRepository.Add(new GemPackageUser()
      {
        UserId = userId,
        Amount = package.Amount,
        GemPackageId = package.Id,
        TimeAdded = DateTime.Now
      }
    );
    return new UnusualSuspectServiceResult<bool>(true);
  }

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

}