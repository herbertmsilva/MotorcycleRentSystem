using Microsoft.AspNetCore.Mvc;
using MediatR;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycleById;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.CreateMotorcycle;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Queries.GetMotorcycles;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.UpdateMotorcycleLicensePlate;
using MotorcycleRentalSystem.Application.UseCases.Motorcycle.Commands.DeleteMotorcycle;
using MotorcycleRentalSystem.Application.DTOs.Motorcycle;
using Asp.Versioning;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Api.Examples;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authorization;

namespace MotorcycleRentalSystem.Api.Controllers
{
    /// <summary>
    /// API para gerenciamento de motocicletas.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Construtor do MotorcycleController.
        /// </summary>
        /// <param name="mediator">Instância de IMediator para mediar comandos e consultas.</param>
        public MotorcycleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria uma nova motocicleta.
        /// </summary>
        /// <remarks>
        /// Uma resposta `400 Bad Request` pode ocorrer devido a um erro de validação de negócios 
        /// (por exemplo, tentando alugar uma motocicleta que já está alugada) ou a um erro de validação 
        /// (por exemplo, campos obrigatórios ausentes).
        /// </remarks>
        /// <param name="command">Comando contendo os detalhes da motocicleta a ser criada.</param>
        /// <returns>Retorna os detalhes da motocicleta recém-criada.</returns>
        /// <response code="201">Retorna os detalhes da motocicleta recém-criada.</response>
        /// <response code="400">Se os dados da motocicleta forem inválidos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateMotorcycleResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [SwaggerResponseExample(201, typeof(MotorcycleExample))]
        [SwaggerResponseExample(400, typeof(GenericBadRequestExample))]
        public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleCommand command)
        {
            var motorcycleDto = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMotorcycleById), new { id = motorcycleDto.Id }, motorcycleDto);
        }

        /// <summary>
        /// Obtém uma motocicleta pelo ID.
        /// </summary>
        /// <param name="id">ID da motocicleta a ser obtida.</param>
        /// <returns>Retorna os detalhes da motocicleta solicitada.</returns>
        /// <response code="200">Retorna os detalhes da motocicleta solicitada.</response>
        /// <response code="404">Se a motocicleta não for encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<MotorcycleDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [SwaggerResponseExample(200, typeof(MotorcycleExample))]
        [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
        public async Task<IActionResult> GetMotorcycleById(Guid id)
        {
            var motorcycle = await _mediator.Send(new GetMotorcycleByIdQuery(id));
            return Ok(motorcycle);
        }

        /// <summary>
        /// Obtém uma lista de motocicletas com a opção de filtrar pela placa.
        /// </summary>
        /// <param name="licensePlateFilter">Filtro opcional para filtrar motocicletas pela placa.</param>
        /// <returns>Retorna uma lista de motocicletas, possivelmente filtrada pela placa.</returns>
        /// <response code="200">Retorna uma lista de motocicletas.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<MotorcycleDto>>), 200)]
        [SwaggerResponseExample(200, typeof(MotorcycleListExample))]
        public async Task<ActionResult<List<MotorcycleDto>>> GetMotorcycles([FromQuery] string? licensePlateFilter)
        {
            var motorcycles = await _mediator.Send(new GetMotorcyclesQuery(licensePlateFilter));
            return Ok(motorcycles);
        }

        /// <summary>
        /// Atualiza a placa de uma motocicleta existente.
        /// </summary>
        /// <remarks>
        /// Uma resposta `400 Bad Request` pode ocorrer devido a um erro de validação de negócios 
        /// ou a um erro de validação de campos (por exemplo, campos obrigatórios ausentes).
        /// </remarks>
        /// <param name="id">ID da motocicleta a ser atualizada.</param>
        /// <param name="command">Comando contendo a nova placa e o ID da motocicleta.</param>
        /// <returns>Retorna NoContent se a atualização for bem-sucedida.</returns>
        /// <response code="204">Se a placa da motocicleta foi atualizada com sucesso.</response>
        /// <response code="400">Se os dados fornecidos forem inválidos.</response>
        /// <response code="404">Se a motocicleta não for encontrada.</response>
        [HttpPatch("{id}/licenseplate")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [SwaggerResponseExample(400, typeof(GenericBadRequestExample))]
        [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
        public async Task<IActionResult> UpdateMotorcycleLicensePlate(Guid id, [FromBody] UpdateMotorcycleLicensePlateCommand command)
        {
            if (id != command.Id)
            {
                throw new NotFoundException($"Motocicleta com ID {command.Id} não encontrada.");
            }

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Remove uma motocicleta pelo ID.
        /// </summary>
        /// <param name="id">ID da motocicleta a ser removida.</param>
        /// <returns>Retorna NoContent se a remoção for bem-sucedida.</returns>
        /// <response code="204">Se a motocicleta foi removida com sucesso.</response>
        /// <response code="404">Se a motocicleta não for encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
        public async Task<IActionResult> DeleteMotorcycle(Guid id)
        {
            await _mediator.Send(new DeleteMotorcycleCommand { Id = id});
            return NoContent();
        }
    }
}
