using AutoMapper;
using CleanCarApi.Application.Cars.Commands;
using CleanCarApi.Application.DTOs;
using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using MediatR;

namespace CleanCarApi.Application.Cars.Handlers
{
    public class CreateCarHandler : IRequestHandler<CreateCarCommand, CarDto>
    {
        private readonly IRepository<Car> _repository;
        private readonly IMapper _mapper;

        public CreateCarHandler(IRepository<Car> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CarDto> Handle(
            CreateCarCommand request,
            CancellationToken cancellationToken)
        {
            // Mappar DTO till entitet
            var car = _mapper.Map<Car>(request.Dto);

            // Sparar i databasen
            await _repository.AddAsync(car);

            // Hämtar den sparade bilen med Brand inkluderad för korrekt mappning
            var created = await _repository.GetByIdAsync(car.Id);

            return _mapper.Map<CarDto>(created!);
        }
    }
}
