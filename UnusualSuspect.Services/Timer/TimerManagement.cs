namespace UnusualSuspect.Services.Timer;

public static class TimerManagement
{
  private static Dictionary<int, System.Timers.Timer> GameTimers = new();
  public static void OnGameTimerStart(int userId, int gameId, TimeSpan timeSpan, Action<int, int> eventHandler)
  {
    if (GameTimers.TryGetValue(gameId, out System.Timers.Timer? timer))
      timer.Stop();
    timer = new System.Timers.Timer(timeSpan);
    timer.AutoReset = false;
    timer.Elapsed += (sender, e) => eventHandler(userId, gameId);
    timer.Start();
    GameTimers[gameId] = timer;
  }

  public static void OnGameTimerStop(int gameId)
  {
    if (GameTimers.TryGetValue(gameId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      GameTimers.Remove(gameId);
    }
  }
  private static Dictionary<int, System.Timers.Timer> UserTimers = new();
  public static void OnUserTimerStart(int userId,TimeSpan timeSpan, Action<int> eventHandler)
  {
    if (UserTimers.TryGetValue(userId, out System.Timers.Timer? timer))
      timer.Stop();
    timer = new System.Timers.Timer(timeSpan);
    timer.AutoReset = false;
    timer.Elapsed += (sender, e) => eventHandler(userId);
    timer.Start();
    UserTimers[userId] = timer;
  }

  public static void OnUserTimerStop(int userId)
  {
    if (UserTimers.TryGetValue(userId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      UserTimers.Remove(userId);
    }
  }

}