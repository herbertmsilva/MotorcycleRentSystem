using FluentValidation;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Commands.ReturnMotorcycle
{
    public class ReturnMotorcycleCommandValidator : AbstractValidator<ReturnMotorcycleCommand>
    {
        public ReturnMotorcycleCommandValidator()
        {
            RuleFor(command => command.RentalId)
                .NotEmpty().WithMessage("RentalId is required.");

            RuleFor(command => command.ReturnDate)
                .GreaterThan(DateTime.MinValue).WithMessage("ReturnDate must be a valid date.")
                .Must(BeAValidReturnDate).WithMessage("ReturnDate cannot be before the start of the rental.");
        }

        private bool BeAValidReturnDate(ReturnMotorcycleCommand command, DateTime returnDate)
        {
            return returnDate > DateTime.Now.AddDays(-1);
        }
    }
}
