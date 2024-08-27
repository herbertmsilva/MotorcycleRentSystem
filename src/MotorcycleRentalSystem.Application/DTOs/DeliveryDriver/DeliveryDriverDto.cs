namespace MotorcycleRentalSystem.Application.DTOs.DeliveryDriver
{
    public class DeliveryDriverDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CNPJ { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNHNumber { get; set; }
        public string CNHType { get; set; }
        public string? CNHImagePath { get; set; }
    }
}
