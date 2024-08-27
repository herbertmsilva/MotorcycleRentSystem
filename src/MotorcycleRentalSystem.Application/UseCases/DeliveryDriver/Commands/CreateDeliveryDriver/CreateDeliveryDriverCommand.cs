using MediatR;
using MotorcycleRentalSystem.Application.DTOs.DeliveryDriver;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.CreateDeliveryDriver
{
    public class CreateDeliveryDriverCommand : IRequest<DeliveryDriverDto>
    {
        public string Name { get; set; } = string.Empty;
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNHNumber { get; set; }
        public string CNHType { get; set; }
    }
}
