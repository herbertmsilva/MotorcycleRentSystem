using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Application.Interfaces.Infrastructure.Settings
{
    public interface IJwtSettings
    {
         string Key { get; }
         string Issuer { get; }
         string Audience { get; }
    }
}
