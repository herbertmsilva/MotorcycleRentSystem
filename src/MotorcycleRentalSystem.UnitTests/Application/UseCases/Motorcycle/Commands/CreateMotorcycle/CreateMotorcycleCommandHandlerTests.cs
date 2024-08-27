using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.Events;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Messaging;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandlerTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly Mock<IRabbitMqClient<MotorcycleRegisteredEvent>> _rabbitMqClientMock;
        private readonly CreateMotorcycleCommandHandler _handler;
        private readonly IMapper _mapper;

        public CreateMotorcycleCommandHandlerTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _rabbitMqClientMock = new Mock<IRabbitMqClient<MotorcycleRegisteredEvent>>();

            var configuration = new MapperConfiguration(cfg => {
                cfg.CreateMap<CreateMotorcycleCommand, MotorcycleEntity>();
                cfg.CreateMap<MotorcycleEntity, MotorcycleDto>();
            });
            _mapper = configuration.CreateMapper();

            _handler = new CreateMotorcycleCommandHandler(
                _motorcycleRepositoryMock.Object,
                _mapper,
                _rabbitMqClientMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateMotorcycleAndPublishEvent()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Year = 2022,
                Model = "Yamaha MT-07",
                LicensePlate = "ABC1234"
            };

            _motorcycleRepositoryMock.Setup(repo => repo.IsLicensePlateUniqueAsync(command.LicensePlate)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _motorcycleRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<MotorcycleEntity>()), Times.Once);
            _rabbitMqClientMock.Verify(client => client.PublishAsync(It.IsAny<MotorcycleRegisteredEvent>(), "motorcycle.registered"), Times.Once);
            Assert.Equal(command.Year, result.Year);
            Assert.Equal(command.Model, result.Model);
            Assert.Equal(command.LicensePlate, result.LicensePlate);

        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLicensePlateNotUnique()
        {
            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Year = 2022,
                Model = "Yamaha MT-07",
                LicensePlate = "ABC1234"
            };

            _motorcycleRepositoryMock.Setup(repo => repo.IsLicensePlateUniqueAsync(command.LicensePlate)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
