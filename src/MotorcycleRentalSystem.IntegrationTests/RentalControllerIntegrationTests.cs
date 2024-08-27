using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.ReturnMotorcycle;
using MotorcycleRentalSystem.Application.UseCases.User.LoginUser;
using System.Net.Http.Json;
using Transferencia.IntegrationTests;
using FluentAssertions;
using MotorcycleRentalSystem.Application.DTOs.User;
using System;
using Microsoft.Extensions.DependencyInjection;
using MotorcycleRentalSystem.Persistence.Context;
using MotorcycleRentalSystem.Application.DTOs.Rental;

namespace MotorcycleRentalSystem.IntegrationTests
{
    public class RentalControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;


        public RentalControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory= factory;
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            var loginCommand = new LoginCommand("deliveryuser", "password123");

            var response = await _client.PostAsJsonAsync("/api/v1/Auth/login", loginCommand);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginDto>>();
            var token = result.Data.Token;

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task RentMotorcycle_ShouldReturnCreated()
        {
            await AuthenticateAsync();
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

            // Arrange
            var rentCommand = new RentMotorcycleCommand
            {
                MotorcycleId = dbContext.Motorcycles.First().Id,
                DeliveryDriverId = dbContext.DeliveryDrivers.First().Id,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(7),
                RentalDays = 7
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/Rental/rent", rentCommand);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var rental = await response.Content.ReadFromJsonAsync<ApiResponse<RentalDto>>();
            rental?.Data.Should().NotBeNull();
            rental?.Data.Motorcycle.Id.Should().Be(rentCommand.MotorcycleId);
            rental?.Data.DeliveryDriver.Id.Should().Be(rentCommand.DeliveryDriverId);
            rental?.Data.IsCompleted.Should().BeFalse();


        }

        [Fact]
        public async Task ReturnMotorcycle_ShouldReturnOkWithoutPenality()
        {
            // Autenticar antes de realizar o teste
            await AuthenticateAsync();

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

            // Arrange
            var rentCommand = new RentMotorcycleCommand
            {
                MotorcycleId = dbContext.Motorcycles.ToList().Last().Id,
                DeliveryDriverId = dbContext.DeliveryDrivers.ToList().Last().Id,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(7),
                RentalDays = 7
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/Rental/rent", rentCommand);
            var rental = await response.Content.ReadFromJsonAsync<ApiResponse<RentalDto>>();
            // Arrange
            var returnCommand = new ReturnMotorcycleCommand
            {
                RentalId = rental.Data.Id, // ID da locação
                ReturnDate = DateTime.UtcNow.AddDays(8) // Data de devolução
            };

            // Act
            var responseReturn = await _client.PostAsJsonAsync("/api/v1.0/Rental/return", returnCommand);

            // Assert
            responseReturn.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var rentalReturn = await responseReturn.Content.ReadFromJsonAsync<ApiResponse<RentalDto>>();
            // Arrange
            rentalReturn?.Data.Should().NotBeNull();
            rentalReturn?.Data.IsCompleted.Should().BeTrue();
            rentalReturn?.Data.HasPenalty.Should().BeFalse();

        }

        [Fact]
        public async Task ReturnMotorcycle_ShouldReturnOkWithPenality()
        {
            // Autenticar antes de realizar o teste
            await AuthenticateAsync();

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

            // Arrange
            var rentCommand = new RentMotorcycleCommand
            {
                MotorcycleId = dbContext.Motorcycles.ToList().Last().Id,
                DeliveryDriverId = dbContext.DeliveryDrivers.ToList().Last().Id,
                ExpectedReturnDate = DateTime.UtcNow.AddDays(7),
                RentalDays = 7
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/Rental/rent", rentCommand);
            var rental = await response.Content.ReadFromJsonAsync<ApiResponse<RentalDto>>();
            // Arrange
            var returnCommand = new ReturnMotorcycleCommand
            {
                RentalId = rental.Data.Id, // ID da locação
                ReturnDate = DateTime.UtcNow.AddDays(9) // Data de devolução
            };

            // Act
            var responseReturn = await _client.PostAsJsonAsync("/api/v1.0/Rental/return", returnCommand);

            // Assert
            responseReturn.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var rentalReturn = await responseReturn.Content.ReadFromJsonAsync<ApiResponse<RentalDto>>();
            // Arrange
            rentalReturn?.Data.Should().NotBeNull();
            rentalReturn?.Data.IsCompleted.Should().BeTrue();
            rentalReturn?.Data.HasPenalty.Should().BeTrue();
            rentalReturn?.Data.PenaltyAmount.Should().BeGreaterThan(0);


        }
    }
}
