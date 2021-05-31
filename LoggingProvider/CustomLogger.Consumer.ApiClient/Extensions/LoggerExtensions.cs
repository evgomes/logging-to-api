using CustomLogger.Consumer.ApiClient.Resources;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace CustomLogger.Consumer.ApiClient.Extensions
{
    /// <summary>
    /// Extension methods to use with the API logger, to send log information to the logging API.
    /// </summary>
    public static class LoggerExtensions
    {
        public static void LogTraceToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            LogToApi(logger, LogLevel.Trace, message, exception, className ?? methodInfo.ReflectedType.Name, methodName ?? methodInfo.Name, user);
        }

        public static void LogDebugToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            LogToApi(logger, LogLevel.Debug, message, exception, className ?? methodInfo.ReflectedType.Name, methodName ?? methodInfo.Name, user);
        }

        public static void LogInformationToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            LogToApi(logger, LogLevel.Information, message, exception, className ?? methodInfo.ReflectedType.Name, methodName ?? methodInfo.Name, user);
        }

        public static void LogWarningToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            LogToApi(logger, LogLevel.Warning, message, exception, className ?? methodInfo.ReflectedType.Name, methodName ?? methodInfo.Name, user);
        }

        public static void LogErrorToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            LogToApi(logger, LogLevel.Error, message, exception, className ?? methodInfo.ReflectedType.Name, methodName ?? methodInfo.Name, user);
        }

        public static void LogCriticalToApi(this ILogger logger, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            LogToApi(logger, LogLevel.Critical, message, exception, className ?? methodInfo.ReflectedType.Name, methodName ?? methodInfo.Name, user);
        }

        public static void LogToApi(this ILogger logger, LogLevel logLevel, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            var state = new LogPayloadResource
            {
                Level = logLevel,
                Message = message,
                Class = className ?? methodInfo.ReflectedType.Name,
                Method = methodName ?? methodInfo.Name,
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
