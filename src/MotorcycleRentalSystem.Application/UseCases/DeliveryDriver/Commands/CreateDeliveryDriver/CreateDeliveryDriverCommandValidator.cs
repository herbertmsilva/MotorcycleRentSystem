using FluentValidation;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommandValidator : AbstractValidator<CreateDeliveryDriverCommand>
    {
        public CreateDeliveryDriverCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

            RuleFor(x => x.CNPJ)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Length(14).WithMessage("O CNPJ deve ter 14 caracteres.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
                .LessThan(DateTime.Today).WithMessage("A data de nascimento deve estar no passado.");

            RuleFor(x => x.CNHNumber)
                .NotEmpty().WithMessage("O número da CNH é obrigatório.")
                .Length(11).WithMessage("O número da CNH deve ter 11 caracteres.");

            RuleFor(x => x.CNHType)
                .NotEmpty().WithMessage("O tipo de CNH é obrigatório.")
                .Must(type => type == "A" || type == "B" || type == "A+B").WithMessage("O tipo de CNH deve ser 'A', 'B' ou 'A+B'.");
        }
    }
}
