using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Enums;
using MotorcycleRentalSystem.Persistence.Context;


namespace Transferencia.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    SeedDataLoginAdmin(db);
                    SeedDataLoginDriver(db);
                    SeedMotorcycle(db);
                    SetDeliveryDriver(db);
                    await db.SaveChangesAsync();

                }
            });
        }

        private void SeedDataLoginAdmin(ApplicationDbContext context) =>
            context.Users.Add(new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                Role= UserRoleEnum.Admin.ToString(),
                Email="teste@teste.com.br"
            });

        private void SeedDataLoginDriver(ApplicationDbContext context) =>
            context.Users.AddRange(new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = "deliveryuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                Role = UserRoleEnum.DeliveryDriver.ToString(),
                Email = "teste@teste.com.br"
            });

        private void SeedMotorcycle(ApplicationDbContext context) =>
            context.Motorcycles.AddRange([
                new MotorcycleEntity
            {
                Id = Guid.NewGuid(),
                CreatedAt= DateTime.UtcNow,
                LicensePlate="ABC1234",
                Model="Moto X",
                Year=2024
            },new MotorcycleEntity{
                Id = Guid.NewGuid(),
                CreatedAt= DateTime.UtcNow,
                LicensePlate="ABC1235",
                Model="Moto y",
                Year=2024
            }
            ]);

        private void SetDeliveryDriver(ApplicationDbContext context) =>
            context.DeliveryDrivers.AddRange([
                new DeliveryDriverEntity
            {
                Id = Guid.NewGuid(),
                CreatedAt= DateTime.UtcNow,
                BirthDate= DateTime.UtcNow.AddYears(-20),
                CNHNumber="12345678910",
                CNPJ="1234567891234",
                CNHType="A",
                Name="Teste",
            },new DeliveryDriverEntity
            {
                Id = Guid.NewGuid(),
                CreatedAt= DateTime.UtcNow,
                BirthDate= DateTime.UtcNow.AddYears(-20),
                CNHNumber="12345678911",
                CNPJ="1234567891232",
                CNHType="A",
                Name="Teste2",
            }
            ]);
    }
}