namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging
{
    public interface IMessageConsumer<T>
    {
        Task HandleAsync(T message);
    }
}
