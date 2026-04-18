using AutoMapper;
using CleanCarApi.Application.Cars.Queries;
using CleanCarApi.Application.DTOs;
using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using MediatR;

namespace CleanCarApi.Application.Cars.Handlers
{
    public class GetCarByIdHandler : IRequestHandler<GetCarByIdQuery, CarDto>
    {
        private readonly IRepository<Car> _repository;
        private readonly IMapper _mapper;

        public GetCarByIdHandler(IRepository<Car> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CarDto> Handle(
            GetCarByIdQuery request,
            CancellationToken cancellationToken)
        {
            // Försöker hämta bilen med angivet id
            var car = await _repository.GetByIdAsync(request.Id);

            // Kastar undantag om bilen inte hittas
            if (car == null)
                throw new KeyNotFoundException($"Car with id {request.Id} not found");

            return _mapper.Map<CarDto>(car);
        }
    }
}
