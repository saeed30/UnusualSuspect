using Microsoft.Extensions.Logging;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.ApiViewModels.Enums.BaseData;
using UnusualSuspect.ApiViewModels.InnerModels;
using UnusualSuspect.ApiViewModels.SignalCommandsData;
using UnusualSuspect.Common.Extensions;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Entities.Identity;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.Contracts.Identity;
using UnusualSuspect.Services.Mapping;

namespace UnusualSuspect.Services.Services;

public sealed class StickerService(INotificationService notificationService,
  IGameService gameService,
  IStickerRepository stickerRepository,
  IStickerPackageRepository stickerPackageRepository,
  IStickerPackageUserRepository stickerPackageUserRepository,
  IApplicationUserManager applicationUserManager,
  IPackageEntityService packageEntityService,
  ILogger<StickerService> logger) : IStickerService
{
  public async Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId, CancellationToken cancellationToken = default)
  {
    if (!await gameService.IsGameParticipantAsync(userId, gameId, cancellationToken))
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
    Sticker? sticker = await stickerRepository.GetByIdAsync(stickerId, cancellationToken);
    if (sticker == null)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidStickerId));
    if (!sticker.IsActive)
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.StickerIsNotActive));
    if (!await UserIsAllowedToUserThisSticker(userId, sticker, cancellationToken))
      return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.StickerIsNotFreeAndNeedToBeBought));
    await notificationService.SendSignalToGameGroup(gameId, SignalCommands.SendSticker, new SendStickerViewModel()
    {
      StickerId = stickerId,
      UserId = userId
    });
    return new UnusualSuspectServiceResult<bool>(true);
  }

  private async Task<bool> UserIsAllowedToUserThisSticker(int userId, Sticker sticker, CancellationToken cancellationToken = default)
  {
    if (sticker.IsFree || !sticker.StickerPackageId.HasValue)
      return true;
    return await stickerPackageUserRepository.OwnedByUserAsync(sticker.StickerPackageId.Value, userId,
      cancellationToken);
  }

  public async Task<UnusualSuspectServiceResult<PackagesGetResponse>> GetPublicPackagesAsync(int userId, CancellationToken cancellationToken = default)
  {
    List<StickerPackage> result = await stickerPackageRepository.GetAllActivePublicAsync(cancellationToken);
    List<PackageDto> packages = result.OrderBy(x => x.ViewOrder).ToPackageDto();
    List<short> ownedPackageIds = await stickerPackageUserRepository.OwnedByUserAsync(userId, cancellationToken);
    foreach (PackageDto packageDto in packages)
      if (ownedPackageIds.Contains((short)packageDto.Id))
        packageDto.Enabled = false;
    return new UnusualSuspectServiceResult<PackagesGetResponse>(
      new PackagesGetResponse()
      {
        PackageDtos = packages
      }
    );
  }

  public async Task<UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>> BuyPackagesAsync(int stickerPackageId, int userId, CancellationToken cancellationToken = default)
  {
    short? packageId = stickerPackageId.ToShort();
    if (!packageId.HasValue)
      return LogicErrorCode.InvalidStickerPackageId;
    StickerPackage? package = await stickerPackageRepository.GetByIdAsync(packageId.Value, cancellationToken);
    if (package == null)
      return LogicErrorCode.InvalidStickerPackageId;
    if (await stickerPackageUserRepository.OwnedByUserAsync(packageId.Value, userId, cancellationToken))
      return LogicErrorCode.AlreadyOwnsThePackage;
    ApplicationUser? user = await applicationUserManager.FindByIdAsync(userId.ToString());
    if (user == null)
      return LogicErrorCode.InvalidUserId;
    var hasEnough = packageEntityService.PayIfHasEnough(package, user, cancellationToken);
    if (!hasEnough.Success)
      return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(hasEnough.Errors);
    if (!hasEnough.Result)
      return LogicErrorCode.DoNotHaveEnoughToPay;
    Guid guid = Guid.NewGuid();
    stickerPackageUserRepository.Add(new StickerPackageUser()
    {
      UserId = userId,
      StickerPackageId = packageId.Value,
      TimeAdded = DateTime.Now,
      Guid = guid
    });
    UnusualSuspectServiceResult<int?> saved = packageEntityService.SavePayment(package, userId, guid);
    if (!saved.Success)
      return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>(saved.Errors);
    return new UnusualSuspectServiceResult<(int?, PriceTypeEnum?)>((package.Amount, (PriceTypeEnum?)package.PriceTypeId));
  }
}