using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle
{
    public class RentMotorcycleCommandHandler(IRentalRepository _rentalRepository,
        IDeliveryDriverRepository _driverRepository,
        IMotorcycleRepository _motorcycleRepository,
        IMapper _mapper)
        : IRequestHandler<RentMotorcycleCommand, RentalDto>
    {

        public async Task<RentalDto> Handle(RentMotorcycleCommand request, CancellationToken cancellationToken)
        {
            await ValidateDriverAsync(request.DeliveryDriverId);
            await ValidateMotorcycleAsync(request.MotorcycleId);

            var startDate = DateTime.UtcNow.AddDays(1);
            var endDate = startDate.AddDays(request.RentalDays);

            var (totalCost, hasPenalty, penaltyAmount) = CalculateTotalCost(request.RentalDays, endDate, request.ExpectedReturnDate);

            var rental = new RentalEntity
            {
                DeliveryDriverId = request.DeliveryDriverId,
                MotorcycleId = request.MotorcycleId,
                StartDate = startDate,
                EndDate = endDate,
                ExpectedEndDate = request.ExpectedReturnDate,
                TotalCost = totalCost,
                HasPenalty = hasPenalty,
                PenaltyAmount = penaltyAmount,
                DailyRate = CalculateDailyRate(request.RentalDays),
                IsCompleted = false
            };

            await _rentalRepository.AddAsync(rental);

            rental = await _rentalRepository.GetByIdAsync(rental.Id, new List<string> { "DeliveryDriver", "Motorcycle" });

            return _mapper.Map<RentalDto>(rental);
        }

        public async Task ValidateDriverAsync(Guid driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);

            if (driver == null)
            {
                throw new NotFoundException($"Entregador com ID {driverId} não encontrado.");
            }

            if (!driver.CNHType.Contains("A"))
            {
                throw new BusinessValidationException("Apenas motoristas com CNH do tipo A podem alugar uma moto.");
            }

            bool isRented = await _rentalRepository.HasActiveRentalAsync(driverId);

            if (isRented)
            {
                throw new BusinessValidationException("A moto selecionada já está alugada.");
            }
        }

        public async Task ValidateMotorcycleAsync(Guid motorcycleId)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null)
            {
                throw new NotFoundException("Moto não encontrada.");
            }

            bool isRented = await _rentalRepository.IsMotorcycleRentedAsync(motorcycleId);
            if (isRented)
            {
                throw new BusinessValidationException("A moto selecionada já está alugada.");
            }
        }

        public (decimal TotalCost, bool HasPenalty, decimal PenaltyAmount) CalculateTotalCost(
                int rentalDays, DateTime endDate, DateTime expectedReturnDate)
        {
            decimal dailyRate = CalculateDailyRate(rentalDays);
            decimal totalCost = dailyRate * rentalDays;
            bool hasPenalty = false;
            decimal penaltyAmount = 0;

            if (expectedReturnDate < endDate)
            {
                // Entrega antecipada
                int unusedDays = (endDate - expectedReturnDate).Days;
                decimal penaltyRate = GetPenaltyRate(rentalDays);
                penaltyAmount = unusedDays * dailyRate * penaltyRate;
                totalCost -= unusedDays * dailyRate;
                totalCost += penaltyAmount;
                hasPenalty = true;
            }
            else if (expectedReturnDate > endDate)
            {
                // Entrega tardia
                int extraDays = (expectedReturnDate - endDate).Days;
                penaltyAmount = extraDays * 50m; // Taxa fixa por dia adicional
                totalCost += penaltyAmount;
                hasPenalty = true;
            }

            return (totalCost, hasPenalty, penaltyAmount);
        }

        private decimal CalculateDailyRate(int rentalDays)
        {
            return rentalDays switch
            {
                7 => 30m,
                15 => 28m,
                30 => 22m,
                45 => 20m,
                50 => 18m,
                _ => throw new ArgumentException("Período de aluguel inválido.")
            };
        }

        private decimal GetPenaltyRate(int rentalDays)
        {
            return rentalDays switch
            {
                7 => 0.2m,
                15 => 0.4m,
                _ => 0m
            };
        }
    }
}
