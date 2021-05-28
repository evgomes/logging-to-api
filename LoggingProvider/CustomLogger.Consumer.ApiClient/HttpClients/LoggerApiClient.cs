using CustomLogger.Consumer.ApiClient.Options;
using CustomLogger.Consumer.ApiClient.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomLogger.Consumer.ApiClient.HttpClients
{
    public class LoggerApiClient : ILoggerApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ApiLoggerOptions _loggerOptions;
        private readonly string _logUrl;

        public const string API_CLIENT_NAME = "CUSTOM_API_LOGGER_API_CLIENT";
        public const string API_CLIENT_ASSEMBLY_NAME = "System.Net.Http.HttpClient.CUSTOM_API_LOGGER_API_CLIENT";

        public LoggerApiClient(HttpClient httpClient, IOptions<ApiLoggerOptions> loggerOptions)
        {
            _httpClient = httpClient;
            _loggerOptions = loggerOptions.Value;

            _logUrl = $"{_loggerOptions.ApiUrl}/api/logs";
        }

        public async Task LogAsync(LogLevel logLevel, string message, Exception exception = null, string className = null, string methodName = null, string user = null)
        {
            var payload = new LogPayloadResource
            {
                Level = logLevel,
                Application = _loggerOptions.ApplicationName,
                Message = message,
                Class = className,
                Method = methodName,
                User = user,
            };

            // Get caller information
            var stackTrace = new StackTrace();
            payload.Stacktrace = stackTrace.ToString();

            var sourceMethod = new StackTrace().GetFrames().Select(frame => frame.GetMethod()).LastOrDefault();
            if (string.IsNullOrEmpty(payload.Class))
            {
                payload.Class = sourceMethod.ReflectedType.FullName;
            }
            if (string.IsNullOrEmpty(payload.Method))
            {
                payload.Method = sourceMethod.Name;
            }

            var json = JsonSerializer.Serialize(payload);
            var serializedContent = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync(_logUrl, serializedContent);
        }
    }
}
