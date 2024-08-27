﻿using MotorcycleRentalSystem.Core.Enums;

namespace MotorcycleRentalSystem.Application.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRoleEnum Role { get; set; }
    }
}
