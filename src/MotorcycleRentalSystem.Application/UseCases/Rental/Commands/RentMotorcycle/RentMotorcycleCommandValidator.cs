using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle
{
    public class RentMotorcycleCommandValidator : AbstractValidator<RentMotorcycleCommand>
    {
        public RentMotorcycleCommandValidator()
        {
            RuleFor(command => command.DeliveryDriverId)
                .NotEmpty().WithMessage("DeliveryDriverId is required.");

            RuleFor(command => command.MotorcycleId)
                .NotEmpty().WithMessage("MotorcycleId is required.");

            RuleFor(command => command.RentalDays)
                .Must(days => days == 7 || days == 15 || days == 30 || days == 45 || days == 50)
                .WithMessage("RentalDays must be one of the following values: 7, 15, 30, 45, 50.");
        }
    }

}
