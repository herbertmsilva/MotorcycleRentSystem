using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.RentMotorcycle;
using MotorcycleRentalSystem.Application.UseCases.Rental.Commands.ReturnMotorcycle;
using Microsoft.AspNetCore.Authorization;
using MotorcycleRentalSystem.Api.Examples;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycleById;
using Swashbuckle.AspNetCore.Filters;
using MotorcycleRentalSystem.Application.UseCases.Rental.Query.GetRentalById;

namespace MotorcycleRentalSystem.Api.Controllers
{
    /// <summary>
    /// API para gerenciar locações de motos.
    /// </summary>
    [Authorize(Policy = "DeliveryOnly")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Construtor para RentalController.
        /// </summary>
        /// <param name="mediator">Instância do Mediator para mediação de comandos.</param>
        public RentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtém uma locação por ID.
        /// </summary>
        /// <param name="id">ID da locação.</param>
        /// <returns>Retorna os detalhes da locação solicitada.</returns>
        /// <response code="200">Retorna os detalhes da locação.</response>
        /// <response code="404">Se a locação não for encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<MotorcycleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [SwaggerResponseExample(200, typeof(MotorcycleExample))]
        [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
        public async Task<IActionResult> GetRentalById(Guid id)
        {
            var motorcycle = await _mediator.Send(new GetRentalByIdQuery(id));
            return Ok(motorcycle);
        }

        /// <summary>
        /// Endpoint para alugar uma moto.
        /// </summary>
        /// <param name="command">Comando contendo os detalhes do aluguel.</param>
        /// <returns>Retorna o ID do aluguel recém-criado.</returns>
        /// <response code="201">Aluguel criado com sucesso.</response>
        /// <response code="400">Se os dados do aluguel forem inválidos.</response>
        [HttpPost("rent")]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RentMotorcycle([FromBody] RentMotorcycleCommand command)
        {
            var rental = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRentalById), new { id = rental.Id }, rental);
        }

        /// <summary>
        /// Endpoint para devolver uma moto e calcular o valor total da locação.
        /// </summary>
        /// <param name="command">Comando contendo os detalhes da devolução.</param>
        /// <returns>Retorna o custo total da locação, incluindo multas, se houver.</returns>
        /// <response code="200">Devolução processada com sucesso.</response>
        /// <response code="400">Se os dados da devolução forem inválidos.</response>
        [HttpPost("return")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReturnMotorcycle([FromBody] ReturnMotorcycleCommand command)
        {
            var rental = await _mediator.Send(command);
            return Ok(rental);
        }
    }
}
