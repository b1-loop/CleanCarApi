using CleanCarApi.Application.DTOs;
using MediatR;

namespace CleanCarApi.Application.Cars.Queries
{
    // Returnerar en enskild bil baserat på id
    public record GetCarByIdQuery(int Id) : IRequest<CarDto>;
}
