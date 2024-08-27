using MotorcycleRentalSystem.Core.Interfaces;
using MotorcycleRentalSystem.Persistence.Context;
using MotorcycleRentalSystem.Core.Entities.Postgres;

namespace MotorcycleRentalSystem.Persistence.Repositories.Postgres
{
    public class DeliveryDriverRepository : RepositoryBase<DeliveryDriverEntity>, IDeliveryDriverRepository
    {
        public DeliveryDriverRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
