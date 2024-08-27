using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Core.Interfaces
{
    public interface IMotorcycleRepository : IRepositoryBase<MotorcycleEntity>
    {
        Task<bool> IsLicensePlateUniqueAsync(string licensePlate);
        
    }
}
