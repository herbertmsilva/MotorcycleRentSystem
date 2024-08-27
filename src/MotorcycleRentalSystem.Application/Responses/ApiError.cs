using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MotorcycleRentalSystem.Application.Responses
{
    public class ApiError
    {
        public Guid ErroId { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }

        public ApiError(Guid errorId, string field, string message)
        {
            ErroId = errorId;
            Field = field;
            Message = message;
        }
    }
}
