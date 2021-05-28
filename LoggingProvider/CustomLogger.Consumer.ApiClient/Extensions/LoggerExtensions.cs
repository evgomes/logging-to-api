using CustomLogger.Consumer.ApiClient.Resources;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace CustomLogger.Consumer.ApiClient.Extensions
{
	/// <summary>
	/// Extension methods to use with the API logger, to save all necessary information to the API database.
	/// </summary>
	public static class LoggerExtensions
	{
		public static void LogTraceToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			LogToApi(logger, LogLevel.Trace, message, exception,className, methodName, user);
		}

		public static void LogDebugToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			LogToApi(logger, LogLevel.Debug, message, exception,className, methodName, user);
		}

		public static void LogInformationToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			LogToApi(logger, LogLevel.Information, message, exception,className, methodName, user);
		}

		public static void LogWarningToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			LogToApi(logger, LogLevel.Warning, message, exception,className, methodName, user);
		}

		public static void LogErrorToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			LogToApi(logger, LogLevel.Error, message, exception,className, methodName, user);
		}

		public static void LogCriticalToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			LogToApi(logger, LogLevel.Critical, message, exception,className, methodName, user);
		}

		public static void LogToApi(this ILogger logger, LogLevel logLevel, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
		{
			var state = new LogPayloadResource
			{
				Level = logLevel,
				Message = message,
				Class = className,
				Method = methodName,
				User = user,
			};

			logger.Log(logLevel, 0, state, exception, LogPayloadResourceFormatter);
		}

		private static string LogPayloadResourceFormatter(LogPayloadResource state, Exception exception)
		{
			if (exception != null)
			{
				return string.Format("{0} - {1} {2}", state.Message, exception.Message, exception.InnerException?.Message ?? string.Empty);
			}

			return state.Message;
		}
	}
}
