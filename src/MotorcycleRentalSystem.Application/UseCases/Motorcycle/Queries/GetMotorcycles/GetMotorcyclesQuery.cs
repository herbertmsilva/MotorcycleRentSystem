using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycles
{
    public class GetMotorcyclesQuery : IRequest<List<MotorcycleDto>>
    {
        public string? LicensePlateFilter { get; set; }

        public GetMotorcyclesQuery(string? licensePlateFilter)
        {
            this.LicensePlateFilter = licensePlateFilter;
        }
    }
}
