using AutoMapper;
using CleanCarApi.Application.DTOs;
using CleanCarApi.Domain.Entities;
using CleanCarApi.Domain.Enums;

namespace CleanCarApi.Application.Mappings
{
    // Profilen talar om för AutoMapper hur entiteter ska mappas till DTOs och tillbaka
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Car -> CarDto: BrandName hämtas från navigationpropertyn Brand.Name
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType.ToString()));

            // CreateCarDto -> Car för att skapa ny entitet från inkommande data
            CreateMap<CreateCarDto, Car>();

            // UpdateCarDto -> Car för att uppdatera befintlig entitet
            CreateMap<UpdateCarDto, Car>();
        }
    }
}
