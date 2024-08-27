using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;
using MotorcycleRentalSystem.Infrastructure.Services;

namespace MotorcycleRentalSystem.Infrastructure.Configurations
{
    public static class MongoDbConfig
    {
        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration["MongoDb:ConnectionString"] ?? throw new ArgumentNullException("MongoDB connection string is not configured.");
            var databaseName = configuration["MongoDb:DatabaseName"] ?? throw new ArgumentNullException("MongoDB databasname string is not configured.");
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);

            services.AddSingleton(mongoClient);
            services.AddSingleton(mongoDatabase);

            services.AddSingleton<IMongoDbService, MongoDbService>();

            return services;
        }
    }
}
