using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.EmployerAPI.Controllers
{
    [Route("api/vacancy")]
    [ApiController]
    public class VacancyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyController(IMapper mapper, IVacancyRepository vacancyRepository)
        {
            _mapper = mapper;
            _vacancyRepository = vacancyRepository;
        }

        [HttpGet]
        [Route("getByCompany/{organizationName}")]
        public async Task<IActionResult> GetVacancyByCompany([FromRoute] string organizationName)
        {
            if(organizationName == null)
            {
                return BadRequest();
            }
            var result = await _vacancyRepository.GetByNameAsync(organizationName);
            List<VacancyDto> vacancies = [];

            foreach(var item in result)
            {
                vacancies.Add(_mapper.Map<VacancyDto>(item));
            }

            return Ok(vacancies);
        }

        [HttpPost]
        [Route("addVacancy")]
        public async Task<IActionResult> AddVacancy([FromBody] VacancyDto request)
        {
            if(request == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Vacancy vacancy = _mapper.Map<Vacancy>(request);
            var result = await _vacancyRepository.CreateAsync(vacancy);
            var response = _mapper.Map<VacancyDto>(result);

            return Ok(response);
        }
    }
}
