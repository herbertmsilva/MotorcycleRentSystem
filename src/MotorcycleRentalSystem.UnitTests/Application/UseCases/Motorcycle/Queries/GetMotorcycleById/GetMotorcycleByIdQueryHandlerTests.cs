using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycleById;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Motorcycle.Queries.GetMotorcycleById
{
    public class GetMotorcycleByIdQueryHandlerTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetMotorcycleByIdQueryHandler _handler;

        public GetMotorcycleByIdQueryHandlerTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<MotorcycleEntity, MotorcycleDto>());
            _mapper = configuration.CreateMapper();

            _handler = new GetMotorcycleByIdQueryHandler(_motorcycleRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsMotorcycleDto()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var motorcycle = new MotorcycleEntity { Id = motorcycleId, Model = "ModelX", LicensePlate = "ABC1234" };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);

            // Act
            var result = await _handler.Handle(new GetMotorcycleByIdQuery (motorcycleId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(motorcycleId, result.Id);
        }

        [Fact]
        public async Task Handle_InvalidId_ThrowsNotFoundException()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(motorcycleId)).ReturnsAsync((MotorcycleEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new GetMotorcycleByIdQuery (motorcycleId), CancellationToken.None));
        }
    }
}
