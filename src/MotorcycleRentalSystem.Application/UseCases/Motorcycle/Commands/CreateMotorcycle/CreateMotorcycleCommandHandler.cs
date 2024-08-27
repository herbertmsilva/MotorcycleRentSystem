using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Events;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler(IMotorcycleRepository _motorcycleRepository, IMapper _mapper, IRabbitMqClient<MotorcycleRegisteredEvent> _rabbitMqClient) : IRequestHandler<CreateMotorcycleCommand, MotorcycleDto>
    {
        public async Task<MotorcycleDto> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            if (!await _motorcycleRepository.IsLicensePlateUniqueAsync(request.LicensePlate))
            {
                throw new BusinessValidationException("Placa deve ser única.");
            }

            var motorcycle = _mapper.Map<MotorcycleEntity>(request);

            await _motorcycleRepository.AddAsync(motorcycle);

            var motorcycleRegisteredEvent = new MotorcycleRegisteredEvent(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.LicensePlate);
            
            await _rabbitMqClient.PublishAsync(motorcycleRegisteredEvent, "motorcycle.registered");

            return _mapper.Map<MotorcycleDto>(motorcycle);
        }
    }
}
