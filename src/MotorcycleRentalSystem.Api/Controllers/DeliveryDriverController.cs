using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDriverById;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using Swashbuckle.AspNetCore.Filters;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Api.Examples;
using MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Queries.GetDeliveryDrivers;
using Microsoft.AspNetCore.Authorization;

namespace MotorcycleRentalSystem.Api.Controllers
{
    /// <summary>
    /// API para gerenciamento de entregadores.
    /// </summary>
    [Authorize(Policy = "DeliveryOnly")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeliveryDriverController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryDriverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria um novo entregador.
        /// </summary>
        /// <remarks>
        /// Uma resposta `400 Bad Request` pode ocorrer devido a erros de validação de campos obrigatórios
        /// ou falhas em regras de negócio.
        /// </remarks>
        /// <param name="command">Comando contendo os detalhes do entregador a ser criado.</param>
        /// <returns>Retorna dados do entregador recém-criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DeliveryDriverDto), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [SwaggerResponseExample(201, typeof(DeliveryDriverExample))] 
        [SwaggerResponseExample(400, typeof(GenericBadRequestExample))]
        [SwaggerResponseExample(500, typeof(GeneralExceptionErrorExample))]
        public async Task<IActionResult> CreateDeliveryDriver([FromBody] CreateDeliveryDriverCommand command)
        {
            var driver = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDeliveryDriverById), new { id = driver.Id }, driver);
        }

        /// <summary>
        /// Atualiza a imagem da CNH de um entregador.
        /// </summary>
        /// <remarks>
        /// Uma resposta `404 Not Found` indica que o entregador especificado não foi encontrado.
        /// Uma resposta `400 Bad Request` indica que a imagem da CNH ou o ID do entregador são inválidos.
        /// </remarks>
        /// <param name="driverId">ID do entregador cuja CNH será atualizada.</param>
        /// <param name="fileName">Nome do arquivo da imagem.</param>
        /// <param name="cnhImageStream">Stream do arquivo da imagem da CNH.</param>
        /// <returns>Retorna NoContent se a operação for bem-sucedida.</returns>
        [HttpPatch("{id}/upload-cnh")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [SwaggerResponseExample(400, typeof(GenericBadRequestExample))]
        [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
        public async Task<IActionResult> UpdateCnhImage(
            Guid id,
            IFormFile cnhImageStream)
        {
            var command = new UpdateCnhImageCommand(id, cnhImageStream, cnhImageStream.FileName);
            var result = await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Obtém uma lista de entregadores.
        /// </summary>
        /// <returns>Retorna uma lista de entregadores cadastrados.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<DeliveryDriverDto>>), 200)]
        [SwaggerResponseExample(200, typeof(ApiResponse<List<DeliveryDriverDto>>))]
        public async Task<IActionResult> GetDeliveryDrivers()
        {
            var query = new GetDeliveryDriversQuery();
            var deliveryDrivers = await _mediator.Send(query);
            return Ok(deliveryDrivers);
        }

        /// <summary>
        /// Obtém detalhes de um entregador pelo ID.
        /// </summary>
        /// <remarks>
        /// Uma resposta `404 Not Found` indica que o entregador especificado não foi encontrado.
        /// </remarks>
        /// <param name="id">ID do entregador a ser recuperado.</param>
        /// <returns>Retorna os detalhes do entregador.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<DeliveryDriverDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [SwaggerResponseExample(404, typeof(NotFoundErrorExample))] 
        public async Task<IActionResult> GetDeliveryDriverById(Guid id)
        {
            var query = new GetDeliveryDriverByIdQuery(id);
            var deliveryDriver = await _mediator.Send(query);
            return Ok(deliveryDriver);
        }
    }
}
