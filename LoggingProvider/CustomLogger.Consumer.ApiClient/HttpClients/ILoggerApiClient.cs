using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CustomLogger.Consumer.ApiClient.HttpClients
{
    public interface ILoggerApiClient
    {
        Task LogAsync(LogLevel logLevel, string message, Exception exception = null, string className = null, string methodName = null, string user = null);
    }
}
