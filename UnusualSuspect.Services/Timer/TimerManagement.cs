namespace UnusualSuspect.Services.Timer;

public static class TimerManagement
{
  private static Dictionary<int, System.Timers.Timer> Timers = new();
  public static void OnTimerStart(int userId, int gameId, TimeSpan timeSpan, Action<int, int> eventHandler)
  {
    if (Timers.TryGetValue(gameId, out System.Timers.Timer? timer))
      timer.Stop();
    timer = new System.Timers.Timer(timeSpan);
    timer.AutoReset = false;
    timer.Elapsed += (sender, e) => eventHandler(userId, gameId);
    timer.Start();
    Timers[gameId] = timer;
  }

  public static void OnTimerStop(int gameId)
  {
    if (Timers.TryGetValue(gameId, out System.Timers.Timer? timer))
    {
      timer.Stop();
      timer.Dispose();
      Timers.Remove(gameId);
    }
  }

}