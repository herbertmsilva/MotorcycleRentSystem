namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(byte[] fileContent, string extension);
    }
}
