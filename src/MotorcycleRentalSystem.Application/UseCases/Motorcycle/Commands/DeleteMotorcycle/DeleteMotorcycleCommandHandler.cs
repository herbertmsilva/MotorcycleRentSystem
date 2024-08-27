using MediatR;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandler(IMotorcycleRepository _motorcycleRepository) : IRequestHandler<DeleteMotorcycleCommand, bool>
    {
        public async Task<bool> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
            if (motorcycle == null)
            {
                throw new NotFoundException($"Entregador com ID {request.Id} não encontrado.");
            }

            await _motorcycleRepository.DeleteAsync(motorcycle);

            return true;
        }
    }
}
