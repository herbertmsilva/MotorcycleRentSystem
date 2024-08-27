using MotorcycleRentalSystem.Core.Interfaces;
using MotorcycleRentalSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Persistence.Repositories.Postgres
{
    public class MotorcycleRepository : RepositoryBase<MotorcycleEntity>, IMotorcycleRepository
    {
        public MotorcycleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> IsLicensePlateUniqueAsync(string licensePlate) => !await _context.Motorcycles.AnyAsync(m => m.LicensePlate.Equals(licensePlate));

       
            
    }
}
