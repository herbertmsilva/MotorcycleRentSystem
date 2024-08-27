using MediatR;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
