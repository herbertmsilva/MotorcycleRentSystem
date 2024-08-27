using Microsoft.AspNetCore.Http;
using Moq;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage
{
    public class UpdateCnhImageCommandHandlerTests
    {
        private readonly Mock<IDeliveryDriverRepository> _deliveryDriverRepositoryMock;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly new Mock<IFormFile> _fileMock;
        private readonly UpdateCnhImageCommandHandler _handler;

        public UpdateCnhImageCommandHandlerTests()
        {
            _deliveryDriverRepositoryMock = new Mock<IDeliveryDriverRepository>();
            _storageServiceMock = new Mock<IStorageService>();

            _fileMock = new Mock<IFormFile>();

            var content = "fake-image-data";
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            _fileMock.Setup(_ => _.OpenReadStream()).Returns(contentStream);
            _fileMock.Setup(_ => _.FileName).Returns("cnh.png");
            _fileMock.Setup(_ => _.Length).Returns(contentStream.Length);
            _fileMock.Setup(_ => _.ContentType).Returns("image/png");

            _handler = new UpdateCnhImageCommandHandler(_deliveryDriverRepositoryMock.Object, _storageServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesDriverCnhImagePath()
        {
            // Arrange
            var driverId = Guid.NewGuid();

            var command = new UpdateCnhImageCommand(driverId, _fileMock.Object, "cnh.png");

            var driver = new DeliveryDriverEntity { Id = driverId };
            _deliveryDriverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId)).ReturnsAsync(driver);
            _storageServiceMock.Setup(service => service.SaveFileAsync(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync("path/to/saved/file");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal("path/to/saved/file", driver.CnhImagePath);
            _deliveryDriverRepositoryMock.Verify(repo => repo.UpdateAsync(driver), Times.Once);
        }

        [Fact]
        public async Task Handle_DriverNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            var command = new UpdateCnhImageCommand(driverId, _fileMock.Object, "cnh.png");

            _deliveryDriverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId)).ReturnsAsync((DeliveryDriverEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
