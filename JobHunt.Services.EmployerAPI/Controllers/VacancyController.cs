using AutoMapper;
using Azure.Core;
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
        private readonly ICompanyRepository _companyRepository;
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyController(IMapper mapper, IVacancyRepository vacancyRepository, ICompanyRepository companyRepository)
        {
            _mapper = mapper;
            _vacancyRepository = vacancyRepository;
            _companyRepository = companyRepository;
        }

        [HttpGet]
        [Route("getAllVacancies")]
        public async Task<IActionResult> GetAllVacancies()
        {
            List<Vacancy> result = await _vacancyRepository.GetAllAsync();
            List<VacancyDto> response = [];
            foreach(var vacancy in result)
            {
                response.Add(_mapper.Map<VacancyDto>(vacancy));
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("getByCompany/{email}")]
        public async Task<IActionResult> GetVacancyByCompany([FromRoute] string email)
        {
            if (email == null)
            {
                return BadRequest();
            }
            var employerDetails = await _companyRepository.GetByEmailAsync(email);

            if(employerDetails == null)
            {
                return NotFound();
            }
            var organizationName = employerDetails.Organization;
            
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
            string email = request.PublishedBy;
            if(email == null)
            {
                return BadRequest();
            }
            var employerDetails = await _companyRepository.GetByEmailAsync(email);
            request.PublishedBy = employerDetails.Organization;

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
