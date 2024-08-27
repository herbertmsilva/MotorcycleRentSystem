using System.Net;

namespace MotorcycleRentalSystem.Application.Exceptions
{
    public class BusinessValidationException : CustomException
    {
        public BusinessValidationException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }
    }
}
