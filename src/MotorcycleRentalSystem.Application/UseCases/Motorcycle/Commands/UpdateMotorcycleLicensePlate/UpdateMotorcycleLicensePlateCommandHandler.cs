
using MediatR;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate
{
    public class UpdateMotorcycleLicensePlateCommandHandler(IMotorcycleRepository _motorcycleRepository) : IRequestHandler<UpdateMotorcycleLicensePlateCommand,bool>
    {
        public async Task<bool> Handle(UpdateMotorcycleLicensePlateCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
            if (motorcycle == null)
            {
                throw new NotFoundException($"Entregador com ID {request.Id} não encontrado.");
            }

            if (!await _motorcycleRepository.IsLicensePlateUniqueAsync(request.NewLicensePlate))
            {
                throw new BusinessValidationException("New license plate must be unique.");
            }

            motorcycle.LicensePlate = request.NewLicensePlate;
            await _motorcycleRepository.UpdateAsync(motorcycle);
            return true;
        }
    }
}
