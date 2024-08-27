namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging
{
    public interface IRabbitMqClient<T>
    {
        Task PublishAsync(T message, string routingKey);
        void Subscribe(Func<T, Task> onMessageReceived);
    }
}
