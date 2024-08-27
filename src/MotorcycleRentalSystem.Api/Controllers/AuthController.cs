using Microsoft.AspNetCore.Mvc;
using MediatR;
using Asp.Versioning;
using MotorcycleRentalSystem.Application.Responses;
using MotorcycleRentalSystem.Api.Examples;
using Swashbuckle.AspNetCore.Filters;
using MotorcycleRentalSystem.Application.UseCases.User.LoginUser;
using MotorcycleRentalSystem.Application.UseCases.User.RegisterUser;
using MotorcycleRentalSystem.Application.DTOs.User;

namespace MotorcycleRentalSystem.Api.Controllers
{
    /// <summary>
    /// API para autenticação e gerenciamento de usuários.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza login de um usuário.
        /// </summary>
        /// <remarks>
        /// Uma resposta `401 Unauthorized` indica que as credenciais fornecidas são inválidas.
        /// </remarks>
        /// <param name="command">Comando contendo as credenciais de login.</param>
        /// <returns>Retorna um token de autenticação.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginDto), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        [SwaggerResponseExample(200, typeof(AuthResponseExample))]
        [SwaggerResponseExample(401, typeof(UnauthorizedErrorExample))]
        [SwaggerResponseExample(500, typeof(GeneralExceptionErrorExample))]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var login = await _mediator.Send(command);
            return Ok(login);
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <remarks>
        /// Uma resposta `400 Bad Request` pode ocorrer devido a erros de validação de campos obrigatórios
        /// ou falhas em regras de negócio.
        /// </remarks>
        /// <param name="command">Comando contendo os detalhes do usuário a ser registrado.</param>
        /// <returns>Retorna uma mensagem de sucesso ou erro.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<object>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 500)]
        //[SwaggerResponseExample(201, typeof(RegisterUserSuccessExample))]
        [SwaggerResponseExample(400, typeof(GenericBadRequestExample))]
        [SwaggerResponseExample(500, typeof(GeneralExceptionErrorExample))]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(Login), new { }, user);
        }
    }
}
