using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycleById
{
    public class GetMotorcycleByIdQuery : IRequest<MotorcycleDto>
    {
        public Guid Id { get; set; }

        public GetMotorcycleByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
