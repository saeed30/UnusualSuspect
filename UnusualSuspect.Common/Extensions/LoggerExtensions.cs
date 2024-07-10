using Microsoft.Extensions.Logging;
using UnusualSuspect.Common.Enums;

namespace UnusualSuspect.Common.Extensions;

public static class LoggerExtensions
{
  public static void LogEvent(this ILogger logger, SystemEventType eventType, int? dataKey,
    string? extraInfo = null, string? message = null,
    LogLevel? logLevel = null, Exception? exception = null, params object?[] paramStrings)
  {
    if (string.IsNullOrWhiteSpace(message))
      message = eventType.ToDisplay();
    logLevel ??= LogLevel.Information;
    using (logger.BeginScope("{DataKey} - {EventType} - {ExtraInfo}", dataKey, eventType, extraInfo))
    {
      logger.Log(logLevel.Value, exception, message, paramStrings);
    }
  }
}