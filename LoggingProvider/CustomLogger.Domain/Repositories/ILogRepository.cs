using CustomLogger.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLogger.Domain.Repositories
{
    public interface ILogRepository
    {
        Task AddAsync(Log log);
        Task<List<Log>> ListAsync();
    }
}
