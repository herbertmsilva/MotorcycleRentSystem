using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Infrastructure.Messaging;
using MotorcycleRentalSystem.Infrastructure.Settings;

namespace MotorcycleRentalSystem.Infrastructure.Configurations
{
    public static class RabbitMqConfig
    {
        public static IServiceCollection AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

            services.AddSingleton<IRabbitMqSettings>(sp => sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value);

            services.AddSingleton(typeof(IRabbitMqClient<>), typeof(RabbitMqClient<>));

            services.AddHostedService<RabbitMqConsumerHostedService>();

            return services;
        }
    }
}
