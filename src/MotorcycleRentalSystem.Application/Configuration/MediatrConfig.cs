using Microsoft.Extensions.DependencyInjection;
using MotorcycleRentalSystem.Application.Behaviors;
using System.Reflection;

namespace MotorcycleRentalSystem.Application.Configuration
{
    public static class MediatrConfig
    {
        public static IServiceCollection AddMediatrConfig(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            return services;
        }
        
    }
}
