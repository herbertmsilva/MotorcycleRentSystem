using FluentValidation.TestHelper;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Rental.Commands.RentMotorcycle
{
    public class RentMotorcycleCommandValidatorTests
    {
        private readonly RentMotorcycleCommandValidator _validator;

        public RentMotorcycleCommandValidatorTests()
        {
            _validator = new RentMotorcycleCommandValidator();
        }

        [Fact]
        public void Validator_ValidCommand_NoValidationErrors()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 7
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_InvalidRentalDays_ValidationError()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 10 // Invalid rental days
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.RentalDays)
                .WithErrorMessage("RentalDays must be one of the following values: 7, 15, 30, 45, 50.");
        }

        [Fact]
        public void Validator_EmptyDriverId_ValidationError()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.Empty,
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 7
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.DeliveryDriverId)
                .WithErrorMessage("DeliveryDriverId is required.");
        }

        [Fact]
        public void Validator_EmptyMotorcycleId_ValidationError()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.Empty,
                RentalDays = 7
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.MotorcycleId)
                .WithErrorMessage("MotorcycleId is required.");
        }
    }
}
