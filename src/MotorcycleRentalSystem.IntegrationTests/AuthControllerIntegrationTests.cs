using MotorcycleRentalSystem.Application.DTOs.User;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Application.UseCases.User.LoginUser;
using System.Net.Http.Json;
using Transferencia.IntegrationTests;
using FluentAssertions;
using MotorcycleRentalSystem.Application.UseCases.User.RegisterUser;
using MotorcycleRentalSystem.Core.Enums;
namespace MotorcycleRentalSystem.IntegrationTests
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_ReturnsOk_WithValidCredentials()
        {
            // Arrange
            var loginCommand = new LoginCommand("testuser", "password123");

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/login", loginCommand);

            // Assert
            response.EnsureSuccessStatusCode();
            var loginDto = await response.Content.ReadFromJsonAsync<ApiResponse<LoginDto>>();
            loginDto?.Data.Should().NotBeNull();
            loginDto?.Data.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Register_ReturnsCreated_WithValidUserData()
        {
            // Arrange
            var registerCommand = new RegisterUserCommand("newuser", "password123", "teste@teste.com.br", UserRoleEnum.Admin);

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/Auth/register", registerCommand);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }
    }
}