using Moq;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.DeleteMotorcycle;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandlerTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly DeleteMotorcycleCommandHandler _handler;

        public DeleteMotorcycleCommandHandlerTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _handler = new DeleteMotorcycleCommandHandler(_motorcycleRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteMotorcycle_WhenMotorcycleExists()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new DeleteMotorcycleCommand { Id = motorcycleId };
            var motorcycle = new MotorcycleEntity { Id = motorcycleId, Model = "Yamaha MT-07", Year = 2022, LicensePlate = "ABC1234" };

            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);
            _motorcycleRepositoryMock.Setup(repo => repo.DeleteAsync(motorcycle)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _motorcycleRepositoryMock.Verify(repo => repo.DeleteAsync(motorcycle), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var command = new DeleteMotorcycleCommand { Id = motorcycleId };

            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync((MotorcycleEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
