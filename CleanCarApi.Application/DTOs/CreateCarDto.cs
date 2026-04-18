using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCarApi.Application.DTOs
{
    // DTO för att skapa en ny bil — innehåller bara det klienten skickar in
    public class CreateCarDto
    {
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int FuelType { get; set; }
        public int BrandId { get; set; }
    }
}
