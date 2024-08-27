using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class MotorcycleListExample : IExamplesProvider<ApiResponse<List<MotorcycleDto>>>
    {
        public ApiResponse<List<MotorcycleDto>> GetExamples()
        {
            return new ApiResponse<List<MotorcycleDto>>
            {
                Success = true,
                Message = "Moto criada com sucesso.",
                Errors = null,
                Data = new List<MotorcycleDto>
                {
                    new MotorcycleDto
                    {
                        Id = Guid.NewGuid(),
                        Year = 2024,
                        Model = "Model X",
                        LicensePlate = "ABC1234"
                    },
                    new MotorcycleDto
                    {
                        Id = Guid.NewGuid(),
                        Year = 2024,
                        Model = "Model Y",
                        LicensePlate = "ABC1235"
                    }
                }
            };
        }
    }
}
