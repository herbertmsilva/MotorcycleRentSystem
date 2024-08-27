using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycles
{
    public class GetMotorcyclesQueryHandler(IMotorcycleRepository _motorcycleRepository, IMapper _mapper) : IRequestHandler<GetMotorcyclesQuery, List<MotorcycleDto>>
    {
       
        public async Task<List<MotorcycleDto>> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            var motorcycles = await _motorcycleRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(request.LicensePlateFilter))
            {
                motorcycles = motorcycles
                    .Where(m => m.LicensePlate.Contains(request.LicensePlateFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return _mapper.Map<List<MotorcycleDto>>(motorcycles);
        }
    }
}
