using FluentValidation.TestHelper;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandValidatorTests
    {
        private readonly CreateMotorcycleCommandValidator _validator;

        public CreateMotorcycleCommandValidatorTests()
        {
            _validator = new CreateMotorcycleCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenYearIsLessThan1900()
        {
            var command = new CreateMotorcycleCommand { Year = 1899 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Year);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenModelIsEmpty()
        {
            var command = new CreateMotorcycleCommand { Model = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Model);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenLicensePlateIsNotInValidFormat()
        {
            var command = new CreateMotorcycleCommand { LicensePlate = "12345678" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.LicensePlate);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new CreateMotorcycleCommand { Year = 2022, Model = "Yamaha MT-07", LicensePlate = "ABC1234" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
