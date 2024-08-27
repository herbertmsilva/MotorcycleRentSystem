using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycleById
{
    public class GetMotorcycleByIdQueryHandler(IMotorcycleRepository _motorcycleRepository, IMapper _mapper) : IRequestHandler<GetMotorcycleByIdQuery, MotorcycleDto>
    {
        public async Task<MotorcycleDto> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(request.Id);
            if (motorcycle == null)
            {
                throw new NotFoundException($"Entregador com ID {request.Id} não encontrado.");
            }

            return _mapper.Map<MotorcycleDto>(motorcycle);
        }
    }
}
