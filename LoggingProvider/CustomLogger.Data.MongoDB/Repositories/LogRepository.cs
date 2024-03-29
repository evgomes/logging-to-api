﻿using CustomLogger.Data.MongoDB.Contexts;
using CustomLogger.Domain.Models;
using CustomLogger.Domain.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLogger.Data.MongoDB.Repositories
{
    public class LogRepository : ILogRepository
    {
        private const string LOG_COLLECTION = "logs";

        private readonly IMongoDbContext _context;
        
        public LogRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Log log)
        {
            await _context.Collection<Log>(LOG_COLLECTION).InsertOneAsync(log);
        }

        public async Task<List<Log>> ListAsync()
        {
            return await _context.Collection<Log>(LOG_COLLECTION).AsQueryable().ToListAsync();
        }
    }
}
