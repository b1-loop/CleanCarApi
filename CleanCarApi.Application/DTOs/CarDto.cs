using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCarApi.Application.DTOs
{
    // DTO (Data Transfer Object) — det som skickas ut från API:et, aldrig entiteten direkt
    public class CarDto
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }

        // Enum visas som text (ex. "Electric") istället för siffra
        public string FuelType { get; set; } = string.Empty;

        // Visar märkets namn direkt istället för bara BrandId
        public string BrandName { get; set; } = string.Empty;
    }
}
