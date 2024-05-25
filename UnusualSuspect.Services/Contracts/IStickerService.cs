using UnusualSuspect.ApiViewModels.InnerModels;

namespace UnusualSuspect.Services.Contracts;

public interface IStickerService
{
  Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId);
  Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default);
}