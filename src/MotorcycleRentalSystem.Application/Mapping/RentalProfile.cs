using AutoMapper;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle;
using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Application.Mapping
{
    public class RentalProfile : Profile
    {
        public RentalProfile() {
            CreateMap<RentMotorcycleCommand, RentalEntity>();
            CreateMap<RentalEntity, RentalDto>();

        }
    }
}
