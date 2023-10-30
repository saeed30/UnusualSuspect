using Microsoft.Extensions.Logging;
using UnusualSuspect.Common.Enums;

namespace UnusualSuspect.Common.Extensions
{
	public static class LoggerExtensions
	{
		public static void LogEvent(this ILogger logger, SystemEventType eventType, string message, int? dataKey = null,
			string? extraInfo = null, LogLevel? logLevel = null, Exception? exception = null)
		{
			logger.LogEvent((int)eventType, message, dataKey, extraInfo, logLevel, exception);
		}
		public static void LogEvent(this ILogger logger, int eventTypeId, string message, int? dataKey = null,
			string? extraInfo = null, LogLevel? logLevel = null, Exception? exception = null)
		{
			if (!Enum.IsDefined(typeof(SystemEventType), eventTypeId))
				throw new Exception("eventTypeId not valid: " + eventTypeId);
			logLevel ??= LogLevel.Information;
			using (logger.BeginScope("{DataKey} - {EventTypeId} - {ExtraInfo}", dataKey, eventTypeId, extraInfo))
			{
				logger.Log(logLevel.Value, exception, message);
			}
		}
	}
}
