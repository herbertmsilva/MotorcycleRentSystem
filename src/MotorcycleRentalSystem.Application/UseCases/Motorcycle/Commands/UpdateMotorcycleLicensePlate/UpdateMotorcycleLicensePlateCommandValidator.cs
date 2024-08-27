using FluentValidation;

namespace MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate
{
    public class UpdateMotorcycleLicensePlateCommandValidator : AbstractValidator<UpdateMotorcycleLicensePlateCommand>
    {
        public UpdateMotorcycleLicensePlateCommandValidator()
        {
            RuleFor(x => x.NewLicensePlate)
            .NotEmpty().WithMessage("A placa é obrigatória.")
            .Matches(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$|^[A-Z]{3}[0-9]{4}$")
            .WithMessage("A placa deve estar em um formato válido (por exemplo, ABC1234 ou ABC1D23).")
            .MaximumLength(7).WithMessage("A placa não pode ter mais de 7 caracteres.");
        }
    }
}
