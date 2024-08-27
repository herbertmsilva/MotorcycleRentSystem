using MediatR;
using MotorcycleRentalSystem.Application.DTOs.User;
using MotorcycleRentalSystem.Core.Enums;

namespace MotorcycleRentalSystem.Application.UseCases.User.RegisterUser
{
    public record RegisterUserCommand(string Username, string Password,string Email, UserRoleEnum Role) : IRequest<UserDto> { }
}
