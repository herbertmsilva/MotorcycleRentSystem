using MediatR;
using MotorcycleRentalSystem.Application.DTOs.User;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.User.LoginUser
{
    public class LoginCommandHandler(IAuthService _authService) : IRequestHandler<LoginCommand, LoginDto>
    {
        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (token == null)
            {
                throw new BusinessValidationException("Wrong username or password.");
            }

            return new LoginDto { Token = token };
        }
    }
}
