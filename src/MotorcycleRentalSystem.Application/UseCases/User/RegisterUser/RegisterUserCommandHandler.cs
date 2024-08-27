using AutoMapper;
using MediatR;
using MotorcycleRentalSystem.Application.DTOs.User;
using MotorcycleRentalSystem.Application.Exceptions;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using MotorcycleRentalSystem.Core.Interfaces;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.UseCases.User.RegisterUser
{
    public class RegisterUserCommandHandler(IUserRepository _userRepository, IMapper _mapper) : IRequestHandler<RegisterUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                throw new BusinessValidationException("Username already exists.");
            }

            // Hash da senha
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new UserEntity
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email=request.Email,
                Role = request.Role.ToString(),
            };

            await _userRepository.AddAsync(newUser);
            return _mapper.Map<UserDto>(newUser);
        }
    }
}
