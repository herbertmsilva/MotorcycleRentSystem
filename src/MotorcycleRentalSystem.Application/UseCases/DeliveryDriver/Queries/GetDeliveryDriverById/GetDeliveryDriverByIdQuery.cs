using MediatR;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDriverById
{
    public class GetDeliveryDriverByIdQuery : IRequest<DeliveryDriverDto>
    {
        public Guid Id { get; set; }

        public GetDeliveryDriverByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
