using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Query.GetRentalById
{
    public class GetRentalByIdQueryHandler(IRentalRepository _rentalRepository,IMapper _mapper) : IRequestHandler<GetRentalByIdQuery, RentalDto>
    {
        public async Task<RentalDto> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetByIdAsync(request.Id);

            if(rental == null)
            {
                throw new NotFoundException("Locação não encontrada.");
            }

            return _mapper.Map<RentalDto>(rental);
        }
    }
}
