using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IStickerService
{
  Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(int userId, CancellationToken cancellationToken = default);
  Task<UnusualSuspectServiceResult<(bool, PriceTypeEnum?)>> BuyPackagesAsync(int stickerPackageId, int userId, CancellationToken cancellationToken = default);
}