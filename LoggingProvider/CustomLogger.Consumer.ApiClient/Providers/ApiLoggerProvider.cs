﻿using CustomLogger.Consumer.ApiClient.Extensions;
using CustomLogger.Consumer.ApiClient.HttpClients;
using CustomLogger.Consumer.ApiClient.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CustomLogger.Consumer.ApiClient.Providers
{
    public class ApiLoggerProvider : ILoggerProvider
    {
        private readonly ILoggerApiClient _loggerApiClient;
        private readonly ApiLoggerOptions _apiLoggerOptions;

        private readonly ConcurrentDictionary<string, ApiLogger> _loggers = new();

        public ApiLoggerProvider(ILoggerApiClient loggerApiClient, IOptions<ApiLoggerOptions> apiLoggerOptions)
        {
            _loggerApiClient = loggerApiClient;
            _apiLoggerOptions = apiLoggerOptions.Value;
        }

        public ILogger CreateLogger(string categoryName)
        {
            // The HttpClientFactory and HttpClient classes use logging behind the scenes. We need to disable logging for these
            // classes to send data to the logging API, otherwise we end up with recursive calls to this method, and consequently
            // we face stack overflow exceptions.
            // See: https://stackoverflow.com/questions/52083818/how-can-i-prevent-httpclient-via-ihttpclientfactory-from-logging-to-ilogger
            if (categoryName.StartsWith(LoggerApiClient.API_CLIENT_ASSEMBLY_NAME))
            {
                return NullLogger.Instance;
            }

            var logLevel = GetLogLevelFor(categoryName);
            return _loggers.GetOrAdd(categoryName, new ApiLogger(_loggerApiClient, logLevel));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        /// <summary>
		/// Returns the minimum log level for a given category, according to the values for the "LogLevels" configuration present 
        /// at appsettings.json.
		/// </summary>
		/// <param name="categoryName">Represents the category name of the logger. This is the assembly name of a type, 
        /// plus a given identification for a type.</param>
		/// <returns>Log level.</returns>
		private LogLevel GetLogLevelFor(string categoryName)
        {
            var keyValuePair = _apiLoggerOptions.LogLevels.FirstOrDefault(e => categoryName.StartsWith(e.Key));
            if(!keyValuePair.IsNull())
            {
                return (LogLevel)Enum.Parse(typeof(LogLevel), keyValuePair.Value);
            }

            return (LogLevel)Enum.Parse(typeof(LogLevel), _apiLoggerOptions.LogLevels["Default"]);
        }
    }
}
