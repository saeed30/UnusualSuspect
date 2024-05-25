using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface ICoinService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);

}