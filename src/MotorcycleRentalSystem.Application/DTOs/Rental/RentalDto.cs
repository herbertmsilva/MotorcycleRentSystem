using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;

namespace MotorcycleRentalSystem.Application.DTOs.Rental
{
    public class RentalDto
    {
        public Guid Id { get; set; }
        public int RentalDays { get; set; }
        public decimal DailyRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public decimal TotalCost { get; set; }
        public bool HasPenalty { get; set; }
        public decimal PenaltyAmount { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime ReturnDate { get; set; }
        public DeliveryDriverDto DeliveryDriver { get; set; }
        public MotorcycleDto Motorcycle { get; set; }
    }
}
