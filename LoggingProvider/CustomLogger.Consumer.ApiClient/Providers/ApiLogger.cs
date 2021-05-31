using CustomLogger.Consumer.ApiClient.HttpClients;
using CustomLogger.Consumer.ApiClient.Resources;
using Microsoft.Extensions.Logging;
using System;

namespace CustomLogger.Consumer.ApiClient.Providers
{
	public class ApiLogger : ILogger
	{
		private readonly ILoggerApiClient _loggerApiClient;
		private readonly LogLevel _logLevel;

		public ApiLogger(ILoggerApiClient loggerApiClient, LogLevel logLevel)
		{
			_loggerApiClient = loggerApiClient;
			_logLevel = logLevel;
		}

		public IDisposable BeginScope<TState>(TState state) => default;

		public bool IsEnabled(LogLevel logLevel) => (logLevel >= _logLevel);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			// Comes from the extension methods that send logs to the API
			var payload = state as LogPayloadResource;
			payload.Message = formatter(state, exception);
			
			// Here I don't await the API call, because HTTP calls can take some time to complete. It's better to use a "fire and forget" approach to not overload the application.
			_loggerApiClient.LogAsync(payload);
		}
	}
}
