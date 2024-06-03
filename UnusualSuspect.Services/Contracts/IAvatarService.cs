using UnusualSuspect.ApiViewModels.Endpoints.Avatar;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IAvatarService
{
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<bool>> BuyPackagesAsync(AvatarPurchaseRequest avatarPurchaseRequest, int userId, CancellationToken cancellationToken = default);
}