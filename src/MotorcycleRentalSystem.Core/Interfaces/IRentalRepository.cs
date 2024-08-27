using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Core.Interfaces
{
    public interface IRentalRepository : IRepositoryBase<RentalEntity>
    {
        Task<bool> IsMotorcycleRentedAsync(Guid motocycleId);
        Task<bool> HasActiveRentalAsync(Guid deliveryDriverId);
    }
}
