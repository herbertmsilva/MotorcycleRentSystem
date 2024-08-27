using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommand : IRequest<MotorcycleDto>
    {
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
