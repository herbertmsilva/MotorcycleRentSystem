using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Application.UseCases.User.LoginUser;
using Transferencia.IntegrationTests;
using MotorcycleRentalSystem.Application.DTOs.User;
namespace MotorcycleRentalSystem.IntegrationTests
{
    public class DeliveryDriverControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DeliveryDriverControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
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
        public async Task CreateDeliveryDriver_ShouldReturnCreated()
        {
            // Autenticar antes de realizar o teste
            await AuthenticateAsync();

            // Arrange
            var command = new CreateDeliveryDriverCommand
            {
                Name = "John Doe",
                CNPJ = "12345678000100",
                BirthDate = new DateTime(1990, 1, 1),
                CNHNumber = "12345678910",
                CNHType = "A",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1.0/DeliveryDriver", command);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var driverResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeliveryDriverDto>>();
            driverResponse.Data.Name.Should().Be("John Doe");
            driverResponse.Data.CNHNumber.Should().Be(command.CNHNumber);
        }

        [Fact]
        public async Task UpdateCnhImage_ShouldReturnNoContent()
        {
            // Autenticar antes de realizar o teste
            await AuthenticateAsync();

            // Arrange
            var createCommand = new CreateDeliveryDriverCommand
            {
                Name = "Jane Doe",
                CNPJ = "12345678000200",
                BirthDate = new DateTime(1990, 2, 2),
                CNHNumber = "09876543211",
                CNHType = "B",
            };
            var createResponse = await _client.PostAsJsonAsync("/api/v1.0/DeliveryDriver", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var createdDriver = await createResponse.Content.ReadFromJsonAsync<ApiResponse<DeliveryDriverDto>>();

            // Criar um stream de imagem simulado
            var cnhImageStream = new MemoryStream(Encoding.UTF8.GetBytes("fake-image-data"));
            var formData = new MultipartFormDataContent();

            // Adicionar o arquivo ao formulário
            formData.Add(new StreamContent(cnhImageStream), "cnhImageStream", "test-cnh.png");

            // Act
            var response = await _client.PatchAsync($"/api/v1.0/DeliveryDriver/{createdDriver.Data.Id}/upload-cnh", formData);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }



        [Fact]
        public async Task GetDeliveryDrivers_ShouldReturnOk()
        {
            // Autenticar antes de realizar o teste
            await AuthenticateAsync();

            // Act
            var response = await _client.GetAsync("/api/v1.0/DeliveryDriver");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var driversResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<DeliveryDriverDto>>>();
            driversResponse.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetDeliveryDriverById_ShouldReturnOk()
        {
            // Autenticar antes de realizar o teste
            await AuthenticateAsync();

            // Arrange
            var createCommand = new CreateDeliveryDriverCommand
            {
                Name = "John Smith",
                CNPJ = "12345678000300",
                BirthDate = new DateTime(1985, 5, 5),
                CNHNumber = "11223344551",
                CNHType = "A+B",

            };
            var createResponse = await _client.PostAsJsonAsync("/api/v1.0/DeliveryDriver", createCommand);
            createResponse.EnsureSuccessStatusCode();

            var createdDriver = await createResponse.Content.ReadFromJsonAsync<ApiResponse<DeliveryDriverDto>>();

            // Act
            var response = await _client.GetAsync($"/api/v1.0/DeliveryDriver/{createdDriver.Data.Id}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var driverResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DeliveryDriverDto>>();
            driverResponse.Data.Id.Should().Be(createdDriver.Data.Id);
        }
    }
}