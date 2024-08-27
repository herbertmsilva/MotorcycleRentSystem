using System;

namespace MotorcycleRentalSystem.Application.DTOs.Motorcycle
{
    public class MotorcycleDto
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }
}
