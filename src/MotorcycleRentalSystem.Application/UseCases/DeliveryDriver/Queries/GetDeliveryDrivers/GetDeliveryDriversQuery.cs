using MediatR;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDrivers
{
    public class GetDeliveryDriversQuery : IRequest<List<DeliveryDriverDto>>
    {
       
    }
}
