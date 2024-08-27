using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class GeneralExceptionErrorExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples()
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "Um erro inesperado ocorreu. Por favor, tente novamente mais tarde.",
                Data = null,
                Errors = new List<ApiError>
            {
                new ApiError (Guid.NewGuid(),"Exception", "Erro interno no servidor.")
            }
            };
        }
    }
}
