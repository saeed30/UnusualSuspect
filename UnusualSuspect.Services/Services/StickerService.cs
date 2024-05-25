using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.ApiViewModels.SignalCommandsData;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class StickerService(INotificationService notificationService,
  IGameService gameService,
  IStickerRepository stickerRepository,
  IStickerPackageRepository stickerPackageRepository,
  ILogger<StickerService> logger) : IStickerService
{
  public async Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId)
  {
    if (!await gameService.IsGameParticipantAsync(userId, gameId))
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    Sticker? sticker = await stickerRepository.GetByIdAsync(stickerId);
    if (sticker == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidStickerId));
    if (!sticker.IsActive)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.StickerIsNotActive));
    if (!UserIsAllowdToUserThisSticker(userId, sticker))
      ;//return error if user do not have access to it
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.SendSticker, new SendStickerViewModel()
    {
      StickerId = stickerId,
      UserId = userId
    });
    return new UnusualSuspectServiceResult<bool>(true);
  }

  private bool UserIsAllowdToUserThisSticker(int userId, Sticker sticker)
  {
    //check is bought or is free
    return true;
  }

  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(CancellationToken cancellationToken = default)
  {
    List<StickerPackage> result = await stickerPackageRepository.GetAllActivePublicAsync(cancellationToken);
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = result.ToPackageDto()
      }
    );
  }
}