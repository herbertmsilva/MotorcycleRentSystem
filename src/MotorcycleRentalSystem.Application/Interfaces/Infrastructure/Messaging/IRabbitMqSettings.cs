namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging
{
    public interface IRabbitMqSettings
    {
        string ConnectionString { get; }
        string ExchangeName { get; }
        string QueueName { get; }
        string RoutingKey { get; }
    }
}
