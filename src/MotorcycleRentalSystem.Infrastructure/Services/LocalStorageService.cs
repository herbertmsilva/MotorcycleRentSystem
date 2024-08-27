using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;

namespace MotorcycleRentalSystem.Infrastructure.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _storagePath;

        public LocalStorageService()
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage");
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> SaveFileAsync(byte[] fileContent, string extension)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_storagePath, fileName);

            await File.WriteAllBytesAsync(filePath, fileContent);
            return filePath;
        }
    }
}