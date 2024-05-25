using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class CoinService(ICoinPackageUserRepository coinRepository, ICoinPackageRepository coinPackageRepository) : ICoinService
{
  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default)
  {
    List<CoinPackage> result = await coinPackageRepository.GetAllActivePublicAsync(cancellationToken);
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = result.ToPackageDto()
      }
    );

  }
}