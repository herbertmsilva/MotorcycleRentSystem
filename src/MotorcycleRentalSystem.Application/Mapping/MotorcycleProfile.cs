using AutoMapper;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle;
using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Application.Mapping
{
    public class MotorcycleProfile : Profile
    {
        public MotorcycleProfile()
        {
            CreateMap<CreateMotorcycleCommand, MotorcycleEntity>();
            CreateMap<MotorcycleEntity, MotorcycleDto>();
        }
    }
}
