using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDriverById;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDriverById
{
    public class GetDeliveryDriverByIdQueryHandlerTests
    {
        private readonly Mock<IDeliveryDriverRepository> _deliveryDriverRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetDeliveryDriverByIdQueryHandler _handler;

        public GetDeliveryDriverByIdQueryHandlerTests()
        {
            _deliveryDriverRepositoryMock = new Mock<IDeliveryDriverRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetDeliveryDriverByIdQueryHandler(_deliveryDriverRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidDriverId_ReturnsDriverDto()
        {
            // Arrange
            var driverId = Guid.NewGuid();
            var driverEntity = new DeliveryDriverEntity { Id = driverId, Name = "John Doe" };
            var driverDto = new DeliveryDriverDto { Id = driverId, Name = "John Doe" };

            _deliveryDriverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId))
                .ReturnsAsync(driverEntity);

            _mapperMock.Setup(mapper => mapper.Map<DeliveryDriverDto>(driverEntity))
                .Returns(driverDto);

            var query = new GetDeliveryDriverByIdQuery(driverId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(driverDto.Id, result.Id);
            Assert.Equal(driverDto.Name, result.Name);
        }

        [Fact]
        public async Task Handle_DriverNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var driverId = Guid.NewGuid();

            _deliveryDriverRepositoryMock.Setup(repo => repo.GetByIdAsync(driverId))
                .ReturnsAsync((DeliveryDriverEntity)null);

            var query = new GetDeliveryDriverByIdQuery(driverId);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
