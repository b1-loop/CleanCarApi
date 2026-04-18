using CleanCarApi.Application.DTOs;
using MediatR;

namespace CleanCarApi.Application.Cars.Queries
{
    // Query är en förfrågan om att läsa data utan att förändra något
    // Returnerar en lista av CarDto
    public record GetAllCarsQuery : IRequest<IEnumerable<CarDto>>;
}
