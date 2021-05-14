using CustomLogger.Data.MongoDB.Contexts.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CustomLogger.Data.MongoDB.Contexts
{
    public class LogDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _db;

        public LogDbContext(IOptions<ConnectionOptions> connectionOptions)
        {
            var client = new MongoClient(connectionOptions.Value.ConnectionString);
            _db = client.GetDatabase(connectionOptions.Value.Database);
        }

        public IMongoCollection<T> Collection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
