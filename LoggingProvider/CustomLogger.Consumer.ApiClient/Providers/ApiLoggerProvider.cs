using CustomLogger.Consumer.ApiClient.HttpClients;
using CustomLogger.Consumer.ApiClient.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            // HttpClientFactory and HttpClient implementations use logging too. We have to disable this since we use these classes to send data to the API,
            // otherwise we end up with recursive calls to the API, and consequently with stack overflow exceptions.
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
		/// Returns the minimum log level for a given category, according to the values for the "LogLevels" configuration present at appsettings.json.
		/// </summary>
		/// <param name="categoryName">Represents the category name of the logger. This is the assembly name of a type, plus a given identification for a type.</param>
		/// <returns>Log level.</returns>
		private LogLevel GetLogLevelFor(string categoryName)
        {
            var keyValuePair = _apiLoggerOptions.LogLevels.FirstOrDefault(e => categoryName.StartsWith(e.Key));
            if (!IsNull(keyValuePair))
            {
                return (LogLevel)Enum.Parse(typeof(LogLevel), keyValuePair.Value);
            }

            return (LogLevel)Enum.Parse(typeof(LogLevel), _apiLoggerOptions.LogLevels["Default"]);
        }

        /// <summary>
		/// Checks if a KeyValuePair is null.
		/// 
		/// Reference: https://stackoverflow.com/questions/1641392/the-default-for-keyvaluepair
		/// </summary>
		/// <typeparam name="TKey">Key type.</typeparam>
		/// <typeparam name="TValue">Value type.</typeparam>
		/// <param name="keyValuePair">KeyValuePair to compare.</param>
		/// <returns>Indication if the value is null.</returns>
		private bool IsNull<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return keyValuePair.Equals(new KeyValuePair<TKey, TValue>());
        }
    }
}
