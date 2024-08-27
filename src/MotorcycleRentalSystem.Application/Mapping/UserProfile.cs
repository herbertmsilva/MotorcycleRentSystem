using AutoMapper;
using MotorcycleRentalSystem.Application.DTOs.User;
using MotorcycleRentalSystem.Application.UseCases.User.RegisterUser;
using MotorcycleRentalSystem.Core.Entities.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserCommand,UserDto>();
            CreateMap<UserEntity,UserDto >();
        }
    }
}
