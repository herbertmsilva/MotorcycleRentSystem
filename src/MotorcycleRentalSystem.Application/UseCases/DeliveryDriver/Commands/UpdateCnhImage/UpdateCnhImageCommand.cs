using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace MotorcycleRentalSystem.Application.UseCases.DeliveryDriver.Commands.UpdateCnhImage
{
    public class UpdateCnhImageCommand : IRequest<bool>
    {
        public Guid DriverId { get; set; }
        public IFormFile CnhImageStream { get; set; }
        public string FileName { get; set; }

        public UpdateCnhImageCommand(Guid driverId, IFormFile cnhImageStream, string fileName)
        {
            DriverId = driverId;
            CnhImageStream = cnhImageStream;
            FileName = fileName;
        }
    }
}
