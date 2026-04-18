using AutoMapper;
using CleanCarApi.Application.Cars.Queries;
using CleanCarApi.Application.DTOs;
using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Interfaces;
using MediatR;
namespace CleanCarApi.Application.Cars.Handlers
{
    // Handler tar emot query och returnerar resultatet
    public class GetAllCarsHandler : IRequestHandler<GetAllCarsQuery, IEnumerable<CarDto>>
    {
        private readonly IRepository<Car> _repository;
        private readonly IMapper _mapper;

        public GetAllCarsHandler(IRepository<Car> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarDto>> Handle(
            GetAllCarsQuery request,
            CancellationToken cancellationToken)
        {
            // Hämtar alla bilar från databasen via repository
            var cars = await _repository.GetAllAsync();

            // Mappar entiteterna till DTOs med AutoMapper
            return _mapper.Map<IEnumerable<CarDto>>(cars);
        }
    }
}
