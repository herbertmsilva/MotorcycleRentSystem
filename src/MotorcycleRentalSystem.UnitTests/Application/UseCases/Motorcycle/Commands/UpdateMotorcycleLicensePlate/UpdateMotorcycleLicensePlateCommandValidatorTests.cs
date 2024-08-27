using FluentValidation.TestHelper;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate
{
    public class UpdateMotorcycleLicensePlateCommandValidatorTests
    {
        private readonly UpdateMotorcycleLicensePlateCommandValidator _validator;

        public UpdateMotorcycleLicensePlateCommandValidatorTests()
        {
            _validator = new UpdateMotorcycleLicensePlateCommandValidator();
        }

        [Fact]
        public void Should_HaveError_When_LicensePlateIsEmpty()
        {
            // Arrange
            var command = new UpdateMotorcycleLicensePlateCommand { NewLicensePlate = "" };

            // Act & Assert
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewLicensePlate)
                  .WithErrorMessage("A placa é obrigatória.");
        }

        [Fact]
        public void Should_HaveError_When_LicensePlateFormatIsInvalid()
        {
            // Arrange
            var command = new UpdateMotorcycleLicensePlateCommand { NewLicensePlate = "12345678" }; // Formato inválido

            // Act & Assert
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewLicensePlate)
                  .WithErrorMessage("A placa deve estar em um formato válido (por exemplo, ABC1234 ou ABC1D23).");
        }

        [Fact]
        public void Should_HaveError_When_LicensePlateExceedsMaxLength()
        {
            // Arrange
            var command = new UpdateMotorcycleLicensePlateCommand { NewLicensePlate = "ABCDE123" }; // Excede 7 caracteres

            // Act & Assert
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.NewLicensePlate)
                  .WithErrorMessage("A placa não pode ter mais de 7 caracteres.");
        }

        [Fact]
        public void Should_NotHaveError_When_LicensePlateIsValid()
        {
            // Arrange
            var command = new UpdateMotorcycleLicensePlateCommand { NewLicensePlate = "ABC1234" };

            // Act & Assert
            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.NewLicensePlate);
        }
    }
}
