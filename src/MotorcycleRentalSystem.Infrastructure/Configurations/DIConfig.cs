using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MotorcycleRentalSystem.Application.Consumers;
using MotorcycleRentalSystem.Application.Events;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;
using MotorcycleRentalSystem.Core.Interfaces;
using MotorcycleRentalSystem.Infrastructure.Messaging;
using MotorcycleRentalSystem.Infrastructure.Services;
using MotorcycleRentalSystem.Persistence.Repositories.Mongo;
using MotorcycleRentalSystem.Persistence.Repositories.Postgres;

namespace MotorcycleRentalSystem.Infrastructure.Configurations
{
    public static class DIConfig
    {
        public static IServiceCollection AddDIConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IDeliveryDriverRepository, DeliveryDriverRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IEventRepository<>), typeof(MongoEventRepository<>));
            services.AddScoped<IMessageConsumer<MotorcycleRegisteredEvent>, MotorcycleConsumer>();

            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddSingleton<ILogRepository, MongoLogRepository>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
