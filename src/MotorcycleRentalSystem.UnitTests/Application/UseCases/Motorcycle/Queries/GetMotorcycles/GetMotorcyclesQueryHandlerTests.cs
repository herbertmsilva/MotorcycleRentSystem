using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycles;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Queries.GetMotorcycles
{
    public class GetMotorcyclesQueryHandlerTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetMotorcyclesQueryHandler _handler;

        public GetMotorcyclesQueryHandlerTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<MotorcycleEntity, MotorcycleDto>());
            _mapper = configuration.CreateMapper();

            _handler = new GetMotorcyclesQueryHandler(_motorcycleRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_NoFilter_ReturnsAllMotorcycles()
        {
            // Arrange
            var motorcycles = new List<MotorcycleEntity>
        {
            new MotorcycleEntity { Model = "ModelX", LicensePlate = "ABC1234" },
            new MotorcycleEntity { Model = "ModelY", LicensePlate = "XYZ5678" }
        };
            _motorcycleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(motorcycles);

            // Act
            var result = await _handler.Handle(new GetMotorcyclesQuery(null), CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Handle_WithFilter_ReturnsFilteredMotorcycles()
        {
            // Arrange
            var motorcycles = new List<MotorcycleEntity>
        {
            new MotorcycleEntity { Model = "ModelX", LicensePlate = "ABC1234" },
            new MotorcycleEntity { Model = "ModelY", LicensePlate = "XYZ5678" }
        };
            _motorcycleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(motorcycles);

            // Act
            var result = await _handler.Handle(new GetMotorcyclesQuery("ABC"), CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("ABC1234", result[0].LicensePlate);
        }

        [Fact]
        public async Task Handle_NoMotorcycles_ReturnsEmptyList()
        {
            // Arrange
            _motorcycleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<MotorcycleEntity>());

            // Act
            var result = await _handler.Handle(new GetMotorcyclesQuery(null), CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
