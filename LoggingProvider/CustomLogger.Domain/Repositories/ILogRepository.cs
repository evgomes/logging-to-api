﻿using CustomLogger.Domain.Models;
using System.Threading.Tasks;

namespace CustomLogger.Domain.Repositories
{
    public interface ILogRepository
    {
        Task AddAsync(Log log);
    }
}
