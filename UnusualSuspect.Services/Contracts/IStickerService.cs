namespace UnusualSuspect.Services.Contracts;

public interface IStickerService
{
  Task<UnusualSuspectServiceResult<bool>> SendStickerToGroupAsync(int userId, short stickerId, int gameId);
}