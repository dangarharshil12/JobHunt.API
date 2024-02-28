using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;

namespace JobHunt.Services.EmployerAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Employer, EmployerDto>().ReverseMap();
                config.CreateMap<Vacancy, VacancyResponseDto>().ReverseMap();
                config.CreateMap<Vacancy, VacancyRequestDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
