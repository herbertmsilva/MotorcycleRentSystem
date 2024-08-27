using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.DeleteMotorcycle;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate;
using System.Net.Http.Json;
using Transferencia.IntegrationTests;
using FluentAssertions;
using MotorcycleRentalSystem.Application.UseCases.User.LoginUser;
using MotorcycleRentalSystem.Application.DTOs.User;
namespace MotorcycleRentalSystem.IntegrationTests
{
    public class MotorcycleControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MotorcycleControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task AuthenticateAsync()
        {
            // Usuário de teste e senha
            var loginCommand = new LoginCommand("testuser", "password123");

            // Obter token JWT
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/login", loginCommand);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginDto>>();
            var token = result.Data.Token;

            // Configurar cabeçalho de autorização com o token JWT
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task CreateMotorcycle_ShouldReturnCreated()
        {
            await AuthenticateAsync();

            // Arrange
            var command = new CreateMotorcycleCommand
            {
                Year = 2023,
                Model = "Yamaha XTZ",
                LicensePlate = "ABH1234"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/Motorcycle", command);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var motorcycleResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CreateMotorcycleResponseDto>>();
            motorcycleResponse?.Data.Model.Should().Be("Yamaha XTZ");
            motorcycleResponse?.Data.LicensePlate.Should().Be("ABH1234");
        }

        [Fact]
        public async Task GetMotorcycleById_ShouldReturnMotorcycle()
        {
            // Arrange
            await AuthenticateAsync();
            var createCommand = new CreateMotorcycleCommand
            {
                Year = 2023,
                Model = "Yamaha XTZ",
                LicensePlate = "ABF1234"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/v1.0/Motorcycle", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var createdMotorcycle = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateMotorcycleResponseDto>>();

            // Act
            var response = await _client.GetAsync($"/api/v1.0/Motorcycle/{createdMotorcycle.Data.Id}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var motorcycle = await response.Content.ReadFromJsonAsync<ApiResponse<MotorcycleDto>>();
            motorcycle.Data.Id.Should().Be(createdMotorcycle.Data.Id);
        }

        [Fact]
        public async Task UpdateMotorcycleLicensePlate_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();
            var createCommand = new CreateMotorcycleCommand
            {
                Year = 2023,
                Model = "Yamaha XTZ",
                LicensePlate = "ABD1234"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/v1.0/Motorcycle", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var createdMotorcycle = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateMotorcycleResponseDto>>();

            var updateCommand = new UpdateMotorcycleLicensePlateCommand
            {
                Id = createdMotorcycle.Data.Id,
                NewLicensePlate = "XYZ5678"
            };

            // Act
            var response = await _client.PatchAsJsonAsync($"/api/v1.0/Motorcycle/{createdMotorcycle.Data.Id}/licenseplate", updateCommand);
            var responseGet = await _client.GetAsync($"/api/v1.0/Motorcycle/{createdMotorcycle.Data.Id}");
            var motorcycle = await responseGet.Content.ReadFromJsonAsync<ApiResponse<MotorcycleDto>>();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            motorcycle.Data.Should().NotBeNull();
            motorcycle.Data.LicensePlate.Should().Be(updateCommand.NewLicensePlate);
        }

        [Fact]
        public async Task DeleteMotorcycle_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();
            var createCommand = new CreateMotorcycleCommand
            {
                Year = 2023,
                Model = "Yamaha XTZ",
                LicensePlate = "ABE1234"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/v1.0/Motorcycle", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var createdMotorcycle = await createResponse.Content.ReadFromJsonAsync<ApiResponse<CreateMotorcycleResponseDto>>();

            // Act
            var response = await _client.DeleteAsync($"/api/v1.0/Motorcycle/{createdMotorcycle.Data.Id}");

            var responseGet = await _client.GetAsync($"/api/v1.0/Motorcycle/{createdMotorcycle.Data.Id}");
            var motorcycle = await responseGet.Content.ReadFromJsonAsync<ApiResponse<MotorcycleDto>>();
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            responseGet.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}