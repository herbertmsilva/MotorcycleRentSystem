using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage
{
    public class UpdateCnhImageCommandValidatorTests
    {
        private readonly UpdateCnhImageCommandValidator _validator;
        private readonly new Mock<IFormFile> _fileMock;

        public UpdateCnhImageCommandValidatorTests()
        {
            _validator = new UpdateCnhImageCommandValidator();

            _fileMock = new Mock<IFormFile>();

            var content = "fake-image-data";
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            _fileMock.Setup(_ => _.OpenReadStream()).Returns(contentStream);
            _fileMock.Setup(_ => _.FileName).Returns("cnh.png");
            _fileMock.Setup(_ => _.Length).Returns(contentStream.Length);
            _fileMock.Setup(_ => _.ContentType).Returns("image/png");
        }

        [Fact]
        public void Validator_ValidCommand_NoValidationErrors()
        {
            // Arrange
            var command = new UpdateCnhImageCommand(Guid.NewGuid(), _fileMock.Object, "cnh.png");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validator_InvalidFileName_ValidationError()
        {
            // Arrange
            _fileMock.Setup(_ => _.FileName).Returns("cnh.txt");
            var command = new UpdateCnhImageCommand(Guid.NewGuid(), _fileMock.Object, "cnh.txt");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.FileName)
                .WithErrorMessage("Somente imagens PNG ou BMP são permitidas.");
        }

        [Fact]
        public void Validator_InvalidImageSize_ValidationError()
        {
            // Arrange
            var largeStream = new MemoryStream(new byte[6 * 1024 * 1024]); // 6 MB
            _fileMock.Setup(_ => _.Length).Returns(largeStream.Length);
            var command = new UpdateCnhImageCommand(Guid.NewGuid(), _fileMock.Object, "cnh.png");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CnhImageStream)
                .WithErrorMessage("O tamanho da imagem deve ser menor que 5 MB.");
        }

        [Fact]
        public void Validator_NullImageStream_ValidationError()
        {
            // Arrange
            var command = new UpdateCnhImageCommand(Guid.NewGuid(), null, "cnh.png");

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.CnhImageStream)
                .WithErrorMessage("O fluxo de imagem da CNH é obrigatório.");
        }
    }
}
