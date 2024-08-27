using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Rental.Commands.RentMotorcycle
{
    public class RentMotorcycleCommandHandlerTests
    {
        private readonly Mock<IRentalRepository> _mockRentalRepository;
        private readonly Mock<IDeliveryDriverRepository> _mockDriverRepository;
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly IMapper _mapper;
        private readonly RentMotorcycleCommandHandler _handler;

        public RentMotorcycleCommandHandlerTests()
        {
            _mockRentalRepository = new Mock<IRentalRepository>();
            _mockDriverRepository = new Mock<IDeliveryDriverRepository>();
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();

            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<RentalEntity, RentalDto>();
            });
            _mapper = configuration.CreateMapper();

            _handler = new RentMotorcycleCommandHandler(
                _mockRentalRepository.Object,
                _mockDriverRepository.Object,
                _mockMotorcycleRepository.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenDriverDoesNotExist()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 7,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(8)
            };

            _mockDriverRepository.Setup(repo => repo.GetByIdAsync(command.DeliveryDriverId))
                                 .ReturnsAsync((DeliveryDriverEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessValidationException_WhenDriverHasInvalidCNH()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 7,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(8)
            };

            var driver = new DeliveryDriverEntity
            {
                Id = command.DeliveryDriverId,
                CNHType = "B"
            };

            _mockDriverRepository.Setup(repo => repo.GetByIdAsync(command.DeliveryDriverId))
                                 .ReturnsAsync(driver);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessValidationException_WhenMotorcycleIsAlreadyRented()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 7,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(8)
            };

            var driver = new DeliveryDriverEntity
            {
                Id = command.DeliveryDriverId,
                CNHType = "A"
            };

            _mockDriverRepository.Setup(repo => repo.GetByIdAsync(command.DeliveryDriverId))
                                 .ReturnsAsync(driver);

            _mockMotorcycleRepository.Setup(repo => repo.GetByIdAsync(command.MotorcycleId))
                                     .ReturnsAsync(new MotorcycleEntity());

            _mockRentalRepository.Setup(repo => repo.IsMotorcycleRentedAsync(command.MotorcycleId))
                                 .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCreateRental_WhenDataIsValid()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                DeliveryDriverId = Guid.NewGuid(),
                MotorcycleId = Guid.NewGuid(),
                RentalDays = 7,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(8)
            };

            var driver = new DeliveryDriverEntity
            {
                Id = command.DeliveryDriverId,
                CNHType = "A"
            };

            var motorcycle = new MotorcycleEntity
            {
                Id = command.MotorcycleId
            };

            var rentalEntity = new RentalEntity
            {
                Id = Guid.NewGuid(),
                DeliveryDriverId = command.DeliveryDriverId,
                MotorcycleId = command.MotorcycleId
            };

            var rentalDto = new RentalDto
            {
                Id = rentalEntity.Id,
                DeliveryDriver = new DeliveryDriverDto { Id= rentalEntity.DeliveryDriverId },
                Motorcycle = new MotorcycleDto { Id = rentalEntity.MotorcycleId }
            };

            _mockDriverRepository.Setup(repo => repo.GetByIdAsync(command.DeliveryDriverId))
                                 .ReturnsAsync(driver);

            _mockMotorcycleRepository.Setup(repo => repo.GetByIdAsync(command.MotorcycleId))
                                     .ReturnsAsync(motorcycle);

            _mockRentalRepository.Setup(repo => repo.IsMotorcycleRentedAsync(command.MotorcycleId))
                                 .ReturnsAsync(false);

            _mockRentalRepository.Setup(repo => repo.HasActiveRentalAsync(command.DeliveryDriverId))
                                 .ReturnsAsync(false);

            _mockRentalRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<List<string>>()))
                                 .ReturnsAsync(rentalEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rentalEntity.Id, result.Id);
            _mockRentalRepository.Verify(repo => repo.AddAsync(It.IsAny<RentalEntity>()), Times.Once);
        }
    }
}
