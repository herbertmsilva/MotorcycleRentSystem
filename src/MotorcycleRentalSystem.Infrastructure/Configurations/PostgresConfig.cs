using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRentalSystem.Persistence.Context;

namespace MotorcycleRentalSystem.Infrastructure.Configurations
{
    public static class PostgresConfig
    {
        public static IServiceCollection AddPostgresConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Postgres")));

            return services;
        }
    }
}
