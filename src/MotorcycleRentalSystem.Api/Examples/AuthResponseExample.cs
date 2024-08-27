using MotorcycleRentalSystem.Application.DTOs.User;
using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{

    public class AuthResponseExample : IExamplesProvider<ApiResponse<LoginDto>>
    {
        public ApiResponse<LoginDto> GetExamples()
        {
            return new ApiResponse<LoginDto>
            {
                Success = false,
                Message = "Não autorizado.",
                Data = new LoginDto { Token ="Token de autorização."},
                Errors = null
            };
        }
    }

}
