using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace MotorcycleRentalSystem.Api.Examples
{
    public class DeliveryDriverExample : IExamplesProvider<ApiResponse<DeliveryDriverDto>>
    {
        public ApiResponse<DeliveryDriverDto> GetExamples()
        {
            return new ApiResponse<DeliveryDriverDto>
            {
                Success = true,
                Message = "Moto criada com sucesso.",
                Data = new DeliveryDriverDto
                {
                    Id = Guid.NewGuid(),
                    Name="",
                    BirthDate = DateTime.Now,
                    CNHImagePath=null,
                    CNHNumber = "123456",
                    CNHType="A", 
                    CNPJ ="123456789122345"
                },
                Errors = null
            };
        }
    }
}
