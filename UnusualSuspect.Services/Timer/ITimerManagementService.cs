
namespace UnusualSuspect.Services.Timer;

public interface ITimerManagementService
{
  void OnGameTimerStart(int gameId, GameTimerEnum gameTimerEnum);
  void OnGameTimerStart(int userId, int gameId, GameTimerEnum gameTimerEnum);
  void OnUserTimerStart(int userId, UserTimerEnum userTimerEnum);
}