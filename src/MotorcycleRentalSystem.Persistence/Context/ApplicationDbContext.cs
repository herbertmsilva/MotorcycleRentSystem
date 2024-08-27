using Microsoft.EntityFrameworkCore;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MotorcycleEntity> Motorcycles { get; set; }
        public DbSet<DeliveryDriverEntity> DeliveryDrivers { get; set; }
        public DbSet<RentalEntity> Rentals { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
