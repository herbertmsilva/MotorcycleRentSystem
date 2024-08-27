using AutoMapper;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver;
using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Application.Mapping
{
    public class DeliveryDriverProfile : Profile
    {
        public DeliveryDriverProfile()
        {
            CreateMap<CreateDeliveryDriverCommand, DeliveryDriverEntity>();
            CreateMap<DeliveryDriverEntity,DeliveryDriverDto>();
        }
    }
}
