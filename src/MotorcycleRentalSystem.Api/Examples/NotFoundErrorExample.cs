using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class NotFoundErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples()
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Recurso não encontrado.",
                Data = null,
                Errors = null
            };
        }
    }
}
