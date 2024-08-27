using Microsoft.Extensions.DependencyInjection;

namespace MotorcycleRentalSystem.Application.Configuration
{
    public static class AutomapperConfig
    {
        public static IServiceCollection AddAutomapperConfig(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
