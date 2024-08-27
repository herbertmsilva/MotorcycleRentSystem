using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle
{
    public class RentMotorcycleCommand : IRequest<RentalDto>
    {
        public Guid DeliveryDriverId { get; set; }
        public Guid MotorcycleId { get; set; }
        public int RentalDays { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
    }
}
