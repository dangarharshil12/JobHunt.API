using AutoMapper;
using Azure.Core;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
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
            List<VacancyResponseDto> response = [];
            foreach(var vacancy in result)
            {
                response.Add(_mapper.Map<VacancyResponseDto>(vacancy));
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("getVacancyById/{id}")]
        public async Task<IActionResult> GetVacancyById([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _vacancyRepository.GetByIdAsync(id);
            var response = _mapper.Map<VacancyResponseDto>(result);

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
                return Ok(null);
            }
            var organizationName = employerDetails.Organization;
            
            var result = await _vacancyRepository.GetByNameAsync(organizationName);
            List<VacancyResponseDto> vacancies = [];

            foreach(var item in result)
            {
                vacancies.Add(_mapper.Map<VacancyResponseDto>(item));
            }

            return Ok(vacancies);
        }

        [HttpPost]
        [Route("addVacancy")]
        public async Task<IActionResult> AddVacancy([FromBody] VacancyRequestDto request)
        {
            string email = request.PublishedBy;
            if(email == null)
            {
                return BadRequest();
            }
            var employerDetails = await _companyRepository.GetByEmailAsync(email);
            request.PublishedBy = employerDetails.Organization;

            if(request == null)
            {
                return BadRequest();
            }

            Vacancy vacancy = _mapper.Map<Vacancy>(request);
            var result = await _vacancyRepository.CreateAsync(vacancy);
            var response = _mapper.Map<VacancyResponseDto>(result);

            return Ok(response);
        }

        [HttpPut]
        [Route("updateVacancy/{id}")]
        public async Task<IActionResult> UpdateVacancy([FromBody] VacancyRequestDto vacancyDto, [FromRoute] Guid id)
        {
            string email = vacancyDto.PublishedBy;
            if (email == null)
            {
                return BadRequest();
            }
            var employerDetails = await _companyRepository.GetByEmailAsync(email);
            vacancyDto.PublishedBy = employerDetails.Organization;
            if (vacancyDto == null)
            {
                return BadRequest();
            }
            Vacancy request = _mapper.Map<Vacancy>(vacancyDto);
            request.Id = id;
            var result = await _vacancyRepository.UpdateAsync(request);

            VacancyResponseDto response = _mapper.Map<VacancyResponseDto>(result);
            return Ok(response);
        }

        [HttpDelete]
        [Route("deleteVacancy/{id}")]
        public async Task<IActionResult> DeleteVacancy(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }

            Vacancy vacancy = await _vacancyRepository.GetByIdAsync(id);
            if(vacancy == null)
            {
                return BadRequest();
            }

            var result = await _vacancyRepository.DeleteAsync(vacancy);
            var response = _mapper.Map<VacancyResponseDto>(result);

            return Ok(response);
        }
    }
}
