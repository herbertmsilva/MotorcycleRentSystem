using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class UnauthorizedErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples()
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Não autorizado.",
                Data = null,
                Errors = null
            };
        }
    }
}
