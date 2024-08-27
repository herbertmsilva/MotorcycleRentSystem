using Moq;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate
{
    public class UpdateMotorcycleLicensePlateCommandHandlerTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly UpdateMotorcycleLicensePlateCommandHandler _handler;

        public UpdateMotorcycleLicensePlateCommandHandlerTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _handler = new UpdateMotorcycleLicensePlateCommandHandler(_motorcycleRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateLicensePlate_WhenMotorcycleExistsAndLicensePlateIsUnique()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new UpdateMotorcycleLicensePlateCommand { Id = motorcycleId, NewLicensePlate = "XYZ1234" };
            var motorcycle = new MotorcycleEntity { Id = motorcycleId, Model = "Yamaha MT-07", Year = 2022, LicensePlate = "ABC1234" };

            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);
            _motorcycleRepositoryMock.Setup(repo => repo.IsLicensePlateUniqueAsync("XYZ1234")).ReturnsAsync(true);
            _motorcycleRepositoryMock.Setup(repo => repo.UpdateAsync(motorcycle)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal("XYZ1234", motorcycle.LicensePlate);
            _motorcycleRepositoryMock.Verify(repo => repo.UpdateAsync(motorcycle), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new UpdateMotorcycleLicensePlateCommand { Id = motorcycleId, NewLicensePlate = "XYZ1234" };

            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync((MotorcycleEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessValidationException_WhenLicensePlateIsNotUnique()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new UpdateMotorcycleLicensePlateCommand { Id = motorcycleId, NewLicensePlate = "XYZ1234" };
            var motorcycle = new MotorcycleEntity { Id = motorcycleId, Model = "Yamaha MT-07", Year = 2022, LicensePlate = "ABC1234" };

            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);
            _motorcycleRepositoryMock.Setup(repo => repo.IsLicensePlateUniqueAsync("XYZ1234")).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
