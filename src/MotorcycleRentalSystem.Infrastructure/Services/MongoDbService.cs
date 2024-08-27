using MongoDB.Driver;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;

namespace MotorcycleRentalSystem.Infrastructure.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
