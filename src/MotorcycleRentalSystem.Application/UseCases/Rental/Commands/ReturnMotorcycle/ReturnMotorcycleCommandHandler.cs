using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.Rental;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;

namespace MotorcycleRentalSystem.Application.UseCases.Rental.Commands.ReturnMotorcycle
{
    public class ReturnMotorcycleCommandHandler(IRentalRepository _rentalRepository, IMapper _mapper) : IRequestHandler<ReturnMotorcycleCommand, RentalDto>
    {
        public async Task<RentalDto> Handle(ReturnMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetByIdAsync(request.RentalId);

            if (rental == null)
            {
                throw new NotFoundException($"Aluguel com ID {request.RentalId} não encontrado.");
            }

            if (rental.IsCompleted)
            {
                throw new BusinessValidationException("Este aluguel já foi concluído.");
            }

            decimal penaltyAmount = 0;
            bool hasPenalty = false;

            if (request.ReturnDate.Date < rental.EndDate.Date)
            {
                // Devolução antecipada
                int unusedDays = (rental.EndDate - request.ReturnDate).Days;
                decimal penaltyRate = GetPenaltyRate(rental.RentalDays);
                penaltyAmount = unusedDays * rental.DailyRate * penaltyRate;
                hasPenalty = true;
            }
            else if (request.ReturnDate.Date > rental.EndDate.Date)
            {
                // Devolução tardia
                int extraDays = (request.ReturnDate - rental.EndDate).Days;
                penaltyAmount = extraDays * 50m; // Exemplo de taxa fixa por dia adicional
                hasPenalty = true;
            }

            rental.ReturnDate = request.ReturnDate;
            rental.PenaltyAmount = penaltyAmount;
            rental.HasPenalty = hasPenalty;
            rental.IsCompleted = true;
            rental.TotalCost += penaltyAmount;

            await _rentalRepository.UpdateAsync(rental);

            return _mapper.Map<RentalDto>(rental);
        }

        private decimal GetPenaltyRate(int rentalDays)
        {
            return rentalDays switch
            {
                7 => 0.2m,  // 20% de multa para planos de 7 dias
                15 => 0.4m, // 40% de multa para planos de 15 dias
                _ => 0m // Sem penalidade para outros planos
            };
        }
    }
}
