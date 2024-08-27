using FluentValidation.TestHelper;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommandValidatorTests
    {
        private readonly CreateDeliveryDriverCommandValidator _validator;

        public CreateDeliveryDriverCommandValidatorTests()
        {
            _validator = new CreateDeliveryDriverCommandValidator();
        }

        [Fact]
        public void Validator_ValidCommand_NoValidationErrors()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Name = "John Doe",
                CNPJ = "12345678901234",
                BirthDate = new DateTime(1990, 1, 1),
                CNHNumber = "12345678901",
                CNHType = "A"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_InvalidName_ValidationError()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Name = "",
                CNPJ = "12345678901234",
                BirthDate = new DateTime(1990, 1, 1),
                CNHNumber = "12345678901",
                CNHType = "A"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage("O nome é obrigatório.");
        }

        [Fact]
        public void Validator_InvalidCNPJ_ValidationError()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Name = "John Doe",
                CNPJ = "12345", // Tamanho inválido
                BirthDate = new DateTime(1990, 1, 1),
                CNHNumber = "12345678901",
                CNHType = "A"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CNPJ)
                .WithErrorMessage("O CNPJ deve ter 14 caracteres.");
        }

        [Fact]
        public void Validator_InvalidCNHType_ValidationError()
        {
            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Name = "John Doe",
                CNPJ = "12345678901234",
                BirthDate = new DateTime(1990, 1, 1),
                CNHNumber = "12345678901",
                CNHType = "C" // Tipo de CNH inválido
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CNHType)
                .WithErrorMessage("O tipo de CNH deve ser 'A', 'B' ou 'A+B'.");
        }
    }
}
