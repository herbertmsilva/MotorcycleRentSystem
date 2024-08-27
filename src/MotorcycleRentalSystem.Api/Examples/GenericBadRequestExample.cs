using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class GenericBadRequestExample : IExamplesProvider<ApiResponse<object>>
    {
        public ApiResponse<object> GetExamples()
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = "A validação falhou para um ou mais campos. / Ocorreu um erro de validação de negócios.",
                Data = null,
                Errors = new List<ApiError>
                {
                    new ApiError(Guid.NewGuid(), "NomeDoCampo", "Mensagem de erro de validação.")
                }
            };
        }
    }
}
