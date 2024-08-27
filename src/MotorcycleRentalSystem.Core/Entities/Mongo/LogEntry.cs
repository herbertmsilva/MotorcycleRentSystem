using MotorcycleRentalSystem.Core.Enums;

namespace MotorcycleRentalSystem.Core.Entities.Mongo
{
    public record LogEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; init; }
        public LogLevel Level { get; init; }  
        public string Message { get; init; }
        public string? Exception { get; init; }
        public string Source { get; init; }
    }
}
