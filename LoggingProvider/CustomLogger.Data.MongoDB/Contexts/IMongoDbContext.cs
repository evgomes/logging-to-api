using MongoDB.Driver;

namespace CustomLogger.Data.MongoDB.Contexts
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> Collection<T>(string name);
    }
}
