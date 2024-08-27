using MediatR;
using MotorcycleRentalSystem.Application.DTOs.User;

namespace MotorcycleRentalSystem.Application.UseCases.User.LoginUser
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginDto>
    {

    }
}
