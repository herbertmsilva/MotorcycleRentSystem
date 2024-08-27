using MongoDB.Driver;

namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services
{
    public interface IMongoDbService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}