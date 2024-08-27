using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MotorcycleRentalSystem.Application.Configuration
{
    public static class FluentValidationConfig
    {
        public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
