using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDriverById;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycles;
using MotorcycleRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDrivers
{
    public class GetDeliveryDriversQueryHandler (IDeliveryDriverRepository _deliveryDriverRepository, IMapper _mapper) :
        IRequestHandler<GetDeliveryDriversQuery, List<DeliveryDriverDto>>
    {
        public async Task<List<DeliveryDriverDto>> Handle(GetDeliveryDriversQuery request, CancellationToken cancellationToken)
        {
            var deliveryDrivers = await _deliveryDriverRepository.GetAllAsync();
            return _mapper.Map<List<DeliveryDriverDto>>(deliveryDrivers);
        }
    }
}
