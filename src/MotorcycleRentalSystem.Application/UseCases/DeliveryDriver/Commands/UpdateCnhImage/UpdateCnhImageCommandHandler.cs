using MediatR;
using Microsoft.AspNetCore.Http;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage
{
    public class UpdateCnhImageCommandHandler : IRequestHandler<UpdateCnhImageCommand, bool>
    {
        private readonly IDeliveryDriverRepository _deliveryDriverRepository;
        private readonly IStorageService _storageService;

        public UpdateCnhImageCommandHandler(IDeliveryDriverRepository deliveryDriverRepository, IStorageService storageService)
        {
            _deliveryDriverRepository = deliveryDriverRepository;
            _storageService = storageService;
        }

        public async Task<bool> Handle(UpdateCnhImageCommand request, CancellationToken cancellationToken)
        {
            var driver = await GetDriverByIdAsync(request.DriverId);
            var filePath = await SaveCnhImageAsync(request.CnhImageStream, request.FileName);
            UpdateDriverCnhImagePath(driver, filePath);

            return await SaveDriverChangesAsync(driver);
        }

        private async Task<DeliveryDriverEntity> GetDriverByIdAsync(Guid driverId)
        {
            var driver = await _deliveryDriverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new NotFoundException($"Entregador com ID {driverId} não encontrado.");
            }
            return driver;
        }

        private async Task<string> SaveCnhImageAsync(IFormFile cnhImageStream, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await cnhImageStream.CopyToAsync(memoryStream);
                var fileContent = memoryStream.ToArray();
                var fileExtension = Path.GetExtension(fileName);
                return await _storageService.SaveFileAsync(fileContent, fileExtension);
            }
        }

        private void UpdateDriverCnhImagePath(DeliveryDriverEntity driver, string filePath)
        {
            driver.CnhImagePath = filePath;
        }

        private async Task<bool> SaveDriverChangesAsync(DeliveryDriverEntity driver)
        {
            await _deliveryDriverRepository.UpdateAsync(driver);
            return true;
        }
    }
}
