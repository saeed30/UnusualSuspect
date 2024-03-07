using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnusualSuspect.ApiViewModels.Enums;
using UnusualSuspect.DataLayer.Contracts.Repository;
using UnusualSuspect.Entities.GameModels;
using UnusualSuspect.Services.Contracts;
using UnusualSuspect.Services.SignalR;

namespace UnusualSuspect.Services.Services
{
  public class StickerService(INotificationService notificationService,
    IGameService gameService,
    IStickerRepository stickerRepository,
    ILogger<StickerService> logger) : IStickerService
  {
    public async Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId)
    {
      if (!await gameService.IsGameParticipantAsync(userId, gameId))
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.UserDoNotParticipateInThisGame));
      Sticker? sticker = await stickerRepository.GetByIdAsync(stickerId);
      if(sticker == null)
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.InvalidStickerId));
      if(!sticker.IsActive)
        return new UnusualSuspectServiceResult<bool>(new UnusualSuspectErrorResult(LogicErrorCode.StickerIsNotActive));
      if (!sticker.IsFree)
        ;//return error if user do not have access to it
      await notificationService.SendSignalToGameGroup(gameId, SignalCommands.SendSticker, stickerId);
      return new UnusualSuspectServiceResult<bool>(true);
    }
  }
}
