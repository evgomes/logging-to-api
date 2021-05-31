using CustomLogger.Consumer.ApiClient.Options;
using CustomLogger.Consumer.ApiClient.Resources;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public const string API_CLIENT_NAME = "CUSTOM_API_LOGGER_API_CLIENT";
        public const string API_CLIENT_ASSEMBLY_NAME = "System.Net.Http.HttpClient.CUSTOM_API_LOGGER_API_CLIENT";

        public LoggerApiClient(HttpClient httpClient, IOptions<ApiLoggerOptions> loggerOptions)
        {
            _httpClient = httpClient;
            _loggerOptions = loggerOptions.Value;

            _logUrl = $"{_loggerOptions.ApiUrl}/api/logs";
        }

        public async Task LogAsync(LogPayloadResource payload)
        {
            try
            {
                payload.Application = _loggerOptions.ApplicationName;

                var json = JsonSerializer.Serialize(payload);
                var serializedContent = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(_logUrl, serializedContent);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error sending log: {0} {1}", ex.Message, ex.InnerException?.Message ?? string.Empty);
            }
        }

        public async Task<List<LogResource>> ListAsync()
        {
            var response = await _httpClient.GetAsync(_logUrl);
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<LogResource>>(stream, _jsonOptions);
        }
    }
}
