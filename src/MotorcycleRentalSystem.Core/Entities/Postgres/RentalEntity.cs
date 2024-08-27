namespace MotorcycleRentalSystem.Core.Entities.Postgres
{
    public class RentalEntity : BaseEntity
    {
        public Guid DeliveryDriverId { get; set; }
        public Guid MotorcycleId { get; set; }
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
        public DeliveryDriverEntity DeliveryDriver { get; set; }
        public MotorcycleEntity Motorcycle { get; set; }
    }
}
