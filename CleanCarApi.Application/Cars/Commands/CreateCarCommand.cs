using CleanCarApi.Application.DTOs;
using MediatR;

namespace CleanCarApi.Application.Cars.Commands
{
    // Command är en förfrågan om att göra något som förändrar data
    // Returnerar den skapade bilen som DTO
    public record CreateCarCommand(CreateCarDto Dto) : IRequest<CarDto>;
}
