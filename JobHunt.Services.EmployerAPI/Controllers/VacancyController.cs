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
