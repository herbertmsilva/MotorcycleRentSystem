using AutoMapper;
using Moq;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.ReturnMotorcycle;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.UnitTests.Application.UseCases.Rental.Commands.ReturnMotorcycle
{
    public class ReturnMotorcycleCommandHandlerTests
        {
            private readonly Mock<IRentalRepository> _mockRentalRepository;
            private readonly Mock<IMapper> _mockMapper;
            private readonly ReturnMotorcycleCommandHandler _handler;

            public ReturnMotorcycleCommandHandlerTests()
            {
                _mockRentalRepository = new Mock<IRentalRepository>();
                _mockMapper = new Mock<IMapper>();

                _handler = new ReturnMotorcycleCommandHandler(
                    _mockRentalRepository.Object,
                    _mockMapper.Object
                );
            }

            [Fact]
            public async Task Handle_ShouldThrowNotFoundException_WhenRentalDoesNotExist()
            {
                // Arrange
                var command = new ReturnMotorcycleCommand
                {
                    RentalId = Guid.NewGuid(),
                    ReturnDate = DateTime.UtcNow
                };

                _mockRentalRepository.Setup(repo => repo.GetByIdAsync(command.RentalId))
                                     .ReturnsAsync((RentalEntity)null);

                // Act & Assert
                await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            }

            [Fact]
            public async Task Handle_ShouldThrowBusinessValidationException_WhenRentalIsAlreadyCompleted()
            {
                // Arrange
                var command = new ReturnMotorcycleCommand
                {
                    RentalId = Guid.NewGuid(),
                    ReturnDate = DateTime.UtcNow
                };

                var rental = new RentalEntity
                {
                    Id = command.RentalId,
                    IsCompleted = true // Já concluído
                };

                _mockRentalRepository.Setup(repo => repo.GetByIdAsync(command.RentalId))
                                     .ReturnsAsync(rental);

                // Act & Assert
                await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, CancellationToken.None));
            }

            [Fact]
            public async Task Handle_ShouldApplyPenaltyForEarlyReturn()
            {
                // Arrange
                var command = new ReturnMotorcycleCommand
                {
                    RentalId = Guid.NewGuid(),
                    ReturnDate = DateTime.UtcNow.AddDays(-2) // Devolução antecipada
                };

                var rental = new RentalEntity
                {
                    Id = command.RentalId,
                    EndDate = DateTime.UtcNow,
                    DailyRate = 30m,
                    RentalDays = 7,
                    IsCompleted = false
                };

                _mockRentalRepository.Setup(repo => repo.GetByIdAsync(command.RentalId))
                                     .ReturnsAsync(rental);

            _mockMapper.Setup(m => m.Map<RentalDto>(It.IsAny<RentalEntity>()))
               .Returns((RentalEntity source) => new RentalDto
               {
                   HasPenalty = source.HasPenalty,
                   PenaltyAmount = source.PenaltyAmount,
                   TotalCost = source.TotalCost
               });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.True(result.HasPenalty);
                Assert.True(result.PenaltyAmount > 0);
                _mockRentalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<RentalEntity>()), Times.Once);
            }

            [Fact]
            public async Task Handle_ShouldApplyPenaltyForLateReturn()
            {
                // Arrange
                var command = new ReturnMotorcycleCommand
                {
                    RentalId = Guid.NewGuid(),
                    ReturnDate = DateTime.UtcNow.AddDays(2) // Devolução tardia
                };

                var rental = new RentalEntity
                {
                    Id = command.RentalId,
                    EndDate = DateTime.UtcNow,
                    DailyRate = 30m,
                    RentalDays = 7,
                    IsCompleted = false
                };

                _mockRentalRepository.Setup(repo => repo.GetByIdAsync(command.RentalId))
                                     .ReturnsAsync(rental);

                _mockMapper.Setup(m => m.Map<RentalDto>(It.IsAny<RentalEntity>()))
                           .Returns((RentalEntity source) => new RentalDto
                           {
                               HasPenalty = source.HasPenalty,
                               PenaltyAmount = source.PenaltyAmount,
                               TotalCost = source.TotalCost
                           });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.True(result.HasPenalty);
                Assert.True(result.PenaltyAmount > 0);
                _mockRentalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<RentalEntity>()), Times.Once);
            }

            [Fact]
            public async Task Handle_ShouldCompleteRentalWithoutPenalty_WhenReturnedOnTime()
            {
                // Arrange
                var command = new ReturnMotorcycleCommand
                {
                    RentalId = Guid.NewGuid(),
                    ReturnDate = DateTime.UtcNow // Devolução na data
                };

                var rental = new RentalEntity
                {
                    Id = command.RentalId,
                    EndDate = DateTime.UtcNow,
                    DailyRate = 30m,
                    RentalDays = 7,
                    IsCompleted = false
                };

                _mockRentalRepository.Setup(repo => repo.GetByIdAsync(command.RentalId))
                                     .ReturnsAsync(rental);

                _mockMapper.Setup(m => m.Map<RentalDto>(It.IsAny<RentalEntity>()))
                           .Returns(new RentalDto());

                // Act
                var result = await _handler.Handle(command, CancellationToken.None);

                // Assert
                Assert.False(result.HasPenalty);
                Assert.Equal(0, result.PenaltyAmount);
                _mockRentalRepository.Verify(repo => repo.UpdateAsync(It.IsAny<RentalEntity>()), Times.Once);
            }
    }
}
