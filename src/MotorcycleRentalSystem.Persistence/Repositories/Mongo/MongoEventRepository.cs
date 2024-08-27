using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MotorcycleRentalSystem.Core.Interfaces;
using System.Collections;

namespace MotorcycleRentalSystem.Persistence.Repositories.Mongo
{
    public class MongoEventRepository<T> : IEventRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoEventRepository(IMongoDatabase mongoDatabase) => _collection = mongoDatabase.GetCollection<T>("Events");

        public async Task AddAsync(T eventItem) => await _collection.InsertOneAsync(eventItem);

        public async Task<IEnumerable<T>> GetAllAsync() => await _collection.Find(new BsonDocument()).ToListAsync();
        
    }
}
