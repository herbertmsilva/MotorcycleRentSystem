using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class MotorcycleExample : IExamplesProvider<ApiResponse<MotorcycleDto>>
    {
        public ApiResponse<MotorcycleDto> GetExamples()
        {
            return new ApiResponse<MotorcycleDto>
            {
                Success = true,
                Message = "Moto criada com sucesso.",
                Data = new MotorcycleDto
                {
                    Id = Guid.NewGuid(),
                    Year = 2024,
                    Model = "Model X",
                    LicensePlate = "ABC1234"
                },
                Errors = null
            };
        }
    }
}
