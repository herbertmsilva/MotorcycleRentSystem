using Microsoft.EntityFrameworkCore;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using MotorcycleRentalSystem.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Persistence.Repositories.Postgres
{
    public class RentalRepository : RepositoryBase<RentalEntity>, IRentalRepository
    {
        public RentalRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<bool> HasActiveRentalAsync(Guid deliveryDriverId) => await _context.Rentals.AnyAsync(r => r.DeliveryDriverId == deliveryDriverId && !r.IsCompleted);
        public async Task<bool> IsMotorcycleRentedAsync(Guid motocycleId) => await _context.Rentals.AnyAsync(r => r.MotorcycleId == motocycleId & !r.IsCompleted);
    }
}
