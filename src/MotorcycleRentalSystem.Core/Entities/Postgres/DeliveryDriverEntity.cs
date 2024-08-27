using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRentalSystem.Core.Entities.Postgres
{
    [Table("DeliveryDrivers")]
    public class DeliveryDriverEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        public string CNPJ { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CNHNumber { get; set; }

        [Required]
        public string CNHType { get; set; } // A, B ou A+B

        public string? CnhImagePath { get; set; }

    }
}
