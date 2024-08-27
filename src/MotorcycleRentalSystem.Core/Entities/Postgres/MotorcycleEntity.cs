using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Core.Entities.Postgres
{
    public class MotorcycleEntity : BaseEntity
    {
        [Required]
        public int Year { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        [StringLength(10)]
        public string LicensePlate { get; set; }
    }
}
