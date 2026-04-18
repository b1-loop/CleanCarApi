
using CleanCarApi.Domain.Enums;
using Microsoft.VisualBasic.FileIO;


namespace CleanCarApi.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public FuelType FuelType { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
    }
}
