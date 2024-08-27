using FluentValidation;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
    {
        public CreateMotorcycleCommandValidator()
        {
            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("O ano deve ser maior que 1900.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("O modelo é obrigatório.")
                .MaximumLength(100).WithMessage("O modelo não pode ter mais de 100 caracteres.");

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("A placa é obrigatória.")
                .Matches(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$|^[A-Z]{3}[0-9]{4}$")
                .WithMessage("A placa deve estar em um formato válido (por exemplo, ABC1234 ou ABC1D23).")
                .MaximumLength(7).WithMessage("A placa não pode ter mais de 7 caracteres.");
        }
    }
}
