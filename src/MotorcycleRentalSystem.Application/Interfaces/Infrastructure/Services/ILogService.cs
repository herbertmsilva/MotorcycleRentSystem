namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services
{
    public interface ILogService
    {
        Task LogInformationAsync(string message);
        Task LogErrorAsync(string message, Exception exception);

    }
}
