using CleanCarApi.Application.Cars.Commands;
using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using MediatR;

namespace CleanCarApi.Application.Cars.Handlers
{
    public class DeleteCarHandler : IRequestHandler<DeleteCarCommand, Unit>
    {
        private readonly IRepository<Car> _repository;

        public DeleteCarHandler(IRepository<Car> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(
            DeleteCarCommand request,
            CancellationToken cancellationToken)
        {
            // Kontrollerar att bilen finns
            var car = await _repository.GetByIdAsync(request.Id);
            if (car == null)
                throw new KeyNotFoundException($"Car with id {request.Id} not found");

            await _repository.DeleteAsync(car);

            // Unit.Value är MediatRs sätt att returnera "ingenting"
            return Unit.Value;
        }
    }
}
