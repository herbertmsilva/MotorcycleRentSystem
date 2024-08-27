using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;

namespace MotorcycleRentalSystem.Infrastructure.Settings
{
    public class RabbitMqSettings : IRabbitMqSettings
    {
        public string ConnectionString { get; set; }
        public string ExchangeName { get; set; }

        public string QueueName { get; set; }

        public string RoutingKey { get; set; }
    }
}
