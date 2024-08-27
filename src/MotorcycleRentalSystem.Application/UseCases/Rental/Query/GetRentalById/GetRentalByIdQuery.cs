using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Rental;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Query.GetRentalById
{
    public record GetRentalByIdQuery(Guid Id) : IRequest<RentalDto>
    {
    }
}
