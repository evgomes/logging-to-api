using CustomLogger.Consumer.ApiClient.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLogger.Consumer.ApiClient.HttpClients
{
    public interface ILoggerApiClient
    {
        Task LogAsync(LogPayloadResource payload);
        Task<List<LogResource>> ListAsync();
    }
}
