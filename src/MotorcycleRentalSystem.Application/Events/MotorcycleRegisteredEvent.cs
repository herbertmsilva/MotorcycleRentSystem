using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.Events
{
    public record MotorcycleRegisteredEvent(Guid MotorcycleId, int Year, string Model, string LicensePlate);
    
}
