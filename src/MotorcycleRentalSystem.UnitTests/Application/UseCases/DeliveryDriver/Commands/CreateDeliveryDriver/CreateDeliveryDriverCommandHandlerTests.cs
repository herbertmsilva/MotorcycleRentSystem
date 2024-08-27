using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommandHandlerTests
    {
        private readonly Mock<IDeliveryDriverRepository> _deliveryDriverRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateDeliveryDriverCommandHandler _handler;

        public CreateDeliveryDriverCommandHandlerTests()
        {
            _deliveryDriverRepositoryMock = new Mock<IDeliveryDriverRepository>();

            var configuration = new MapperConfiguration(cfg => {
                    cfg.CreateMap<CreateDeliveryDriverCommand, DeliveryDriverEntity>();
                    cfg.CreateMap<DeliveryDriverEntity, DeliveryDriverDto>();
            });
            _mapper = configuration.CreateMapper();

            _handler = new CreateDeliveryDriverCommandHandler(_deliveryDriverRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesDeliveryDriver()
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
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _deliveryDriverRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<DeliveryDriverEntity>()), Times.Once);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.CNPJ, result.CNPJ);
            Assert.Equal(command.BirthDate.ToUniversalTime(), result.BirthDate);
            Assert.Equal(command.CNHNumber, result.CNHNumber);
            Assert.Equal(command.CNHType, result.CNHType);
        }
    }
}
