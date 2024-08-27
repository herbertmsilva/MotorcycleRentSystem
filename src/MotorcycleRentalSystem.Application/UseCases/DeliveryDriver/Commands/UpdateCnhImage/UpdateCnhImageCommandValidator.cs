using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage
{
    public class UpdateCnhImageCommandValidator : AbstractValidator<UpdateCnhImageCommand>
    {
        public UpdateCnhImageCommandValidator()
        {
            RuleFor(x => x.CnhImageStream)
                .NotNull().WithMessage("O fluxo de imagem da CNH é obrigatório.")
                .Must(BeAValidSize).WithMessage("O tamanho da imagem deve ser menor que 5 MB.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("O nome do arquivo é obrigatório.")
                .Must(BeAValidImageType).WithMessage("Somente imagens PNG ou BMP são permitidas.");
        }

        private bool BeAValidImageType(string fileName)
        {
            var allowedExtensions = new[] { ".png", ".jpg", "jpeg" };
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }

        private bool BeAValidSize(IFormFile cnhImageStream)
        {
            const long maxSizeInBytes = 5 * 1024 * 1024; // 5 MB
            try
            {
                if (cnhImageStream.Length <= maxSizeInBytes)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
