using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommandHandler(IDeliveryDriverRepository _deliveryDriverRepository, IMapper _mapper)
        : IRequestHandler<CreateDeliveryDriverCommand, DeliveryDriverDto>
    {
        public async Task<DeliveryDriverDto> Handle(CreateDeliveryDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = _mapper.Map<DeliveryDriverEntity>(request);

            if (driver.BirthDate.Kind != DateTimeKind.Utc)
            {
                driver.BirthDate = driver.BirthDate.ToUniversalTime();
            }

            await _deliveryDriverRepository.AddAsync(driver);
            return _mapper.Map<DeliveryDriverDto>(driver);
            
        }
    }
}
