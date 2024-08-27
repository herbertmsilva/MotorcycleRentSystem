using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDriverById
{
    public class GetDeliveryDriverByIdQueryHandler(IDeliveryDriverRepository _deliveryDriverRepository, IMapper _mapper) :
        IRequestHandler<GetDeliveryDriverByIdQuery, DeliveryDriverDto>
    {
        public async Task<DeliveryDriverDto> Handle(GetDeliveryDriverByIdQuery request, CancellationToken cancellationToken)
        {
            var driver = await _deliveryDriverRepository.GetByIdAsync(request.Id);

            if (driver == null)
            {
                throw new NotFoundException($"Entregador com ID {request.Id} não encontrado.");
            }

            return _mapper.Map<DeliveryDriverDto>(driver);
        }
    }
}
