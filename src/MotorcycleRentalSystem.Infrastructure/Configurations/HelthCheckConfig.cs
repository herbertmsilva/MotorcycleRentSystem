using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HealthChecks.MongoDb;
using HealthChecks.NpgSql;
using HealthChecks.RabbitMQ;
using MongoDB.Driver;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MotorcycleRentalSystem.Infrastructure.Configurations
{
    public static class HelthCheckConfig
    {
        public static IServiceCollection AddHelthCheckConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var npgConnectionString = configuration.GetConnectionString("Postgres") 
                ?? throw new ArgumentNullException("Postgres connection string is not configured.");

            var rabbitMqConnectionString = configuration["RabbitMq:ConnectionString"]
                ?? throw new ArgumentNullException("RabbitMq connection string is not configured.");

            services.AddHealthChecks()
                .AddCheck("Self", () => HealthCheckResult.Healthy())
                .AddNpgSql(npgConnectionString, name: "Postgres DB", failureStatus: HealthStatus.Unhealthy)
                .AddRabbitMQ(rabbitConnectionString: rabbitMqConnectionString, name: "RabbitMQ");

            return services;
        }
    }
}
