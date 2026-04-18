using AutoMapper;
using CleanCarApi.Application.Cars.Commands;
using CleanCarApi.Application.DTOs;
using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using MediatR;

namespace CleanCarApi.Application.Cars.Handlers
{
    public class UpdateCarHandler : IRequestHandler<UpdateCarCommand, CarDto>
    {
        private readonly IRepository<Car> _repository;
        private readonly IMapper _mapper;

        public UpdateCarHandler(IRepository<Car> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CarDto> Handle(
            UpdateCarCommand request,
            CancellationToken cancellationToken)
        {
            // Kontrollerar att bilen finns
            var car = await _repository.GetByIdAsync(request.Id);
            if (car == null)
                throw new KeyNotFoundException($"Car with id {request.Id} not found");

            // Skriver över entitetens värden med data från DTO
            _mapper.Map(request.Dto, car);

            await _repository.UpdateAsync(car);

            var updated = await _repository.GetByIdAsync(car.Id);
            return _mapper.Map<CarDto>(updated!);
        }
    }
}
