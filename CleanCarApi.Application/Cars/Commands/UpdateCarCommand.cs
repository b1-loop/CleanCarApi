using CleanCarApi.Application.DTOs;
using MediatR;

namespace CleanCarApi.Application.Cars.Commands
{
    // Innehåller id för bilen som ska uppdateras + den nya datan
    public record UpdateCarCommand(int Id, UpdateCarDto Dto) : IRequest<CarDto>;
}
