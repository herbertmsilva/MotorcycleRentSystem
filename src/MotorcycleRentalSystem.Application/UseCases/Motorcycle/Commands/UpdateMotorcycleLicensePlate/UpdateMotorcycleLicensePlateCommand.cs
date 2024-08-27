using MediatR;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate
{
    public class UpdateMotorcycleLicensePlateCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string NewLicensePlate { get; set; }
    }
}
