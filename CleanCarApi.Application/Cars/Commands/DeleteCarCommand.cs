using MediatR;


namespace CleanCarApi.Application.Cars.Commands
{
    // Tar bara ett id — returnerar Unit vilket betyder "ingenting" (void för MediatR)
    public record DeleteCarCommand(int Id) : IRequest<Unit>;
}
