using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<ApiError> Errors { get; set; }

        public ApiResponse()
        {
        }

        public ApiResponse(T data, string message = null)
        {
            Success = true;
            Message = message ?? "Operation completed successfully.";
            Data = data;
            Errors = null;
        }

        public ApiResponse(string message)
        {
            Success = false;
            Message = message;
            Data = default;
            Errors = new List<ApiError>();
        }

        public ApiResponse(List<ApiError> errors, string message = "Validation failed")
        {
            Success = false;
            Message = message;
            Data = default;
            Errors = errors;
        }
    }

    
}
