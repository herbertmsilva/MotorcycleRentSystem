using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Commands.ReturnMotorcycle
{
    public class ReturnMotorcycleCommand : IRequest<RentalDto>
    {
        public Guid RentalId { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
