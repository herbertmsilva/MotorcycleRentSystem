using Microsoft.AspNetCore.Http;
using System.Text.Json;
using MotorcycleRentalSystem.Application.Responses;

namespace MotorcycleRentalSystem.Infrastructure.Middleware
{
    public class SuccessResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public SuccessResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Capturar o corpo da resposta
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var bodyText = await new StreamReader(responseBody).ReadToEndAsync();
                    responseBody.Seek(0, SeekOrigin.Begin);

                    ApiResponse<object> apiResponse = null;

                    if (string.IsNullOrEmpty(bodyText))
                    {
                        apiResponse = new ApiResponse<object>()
                        {
                            Success = true,
                            Message = "Operação concluída com sucesso."
                        };
                    }
                    else
                    {
                        apiResponse = new ApiResponse<object>(JsonSerializer.Deserialize<object>(bodyText))
                        {
                            Success = true,
                            Message = "Operação concluída com sucesso."
                        };
                    }

                    context.Response.ContentType = "application/json";
                    var jsonResponse = JsonSerializer.Serialize(apiResponse);

                    await context.Response.WriteAsync(jsonResponse);
                }

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
