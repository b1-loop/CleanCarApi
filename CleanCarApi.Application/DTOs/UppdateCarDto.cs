using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCarApi.Application.DTOs
{
   // DTO för att uppdatera en bil
public class UpdateCarDto
    {
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int FuelType { get; set; }
        public int BrandId { get; set; }
    }
}
