using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Application.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Infrastructure.Messaging
{
    public class RabbitMqConsumerHostedService : BackgroundService
    {
        private readonly IRabbitMqClient<MotorcycleRegisteredEvent> _rabbitMqClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RabbitMqConsumerHostedService> _logger;

        public RabbitMqConsumerHostedService(
            IRabbitMqClient<MotorcycleRegisteredEvent> rabbitMqClient,
            IServiceScopeFactory scopeFactory,
            ILogger<RabbitMqConsumerHostedService> logger)
        {
            _rabbitMqClient = rabbitMqClient;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitMqClient.Subscribe( async (eventMessage) =>
            {
                _logger.LogInformation($"Evento de moto registrado recebido: {eventMessage}");

                using (var scope = _scopeFactory.CreateScope())
                {
                    var consumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer<MotorcycleRegisteredEvent>>();
                    try
                    {
                        await consumer.HandleAsync(eventMessage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar evento de moto registrada.");
                    }
                }
            });

            return Task.CompletedTask; // Serviço contínuo
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Hosted Service está parando.");
            return base.StopAsync(stoppingToken);
        }
    }
}
