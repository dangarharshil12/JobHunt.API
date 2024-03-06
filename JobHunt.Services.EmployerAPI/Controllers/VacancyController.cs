using AutoMapper;
using Azure.Core;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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
        protected ResponseDto _response;

        public VacancyController(IMapper mapper, IVacancyRepository vacancyRepository, ICompanyRepository companyRepository)
        {
            _mapper = mapper;
            _vacancyRepository = vacancyRepository;
            _companyRepository = companyRepository;
            _response = new();
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
            _response.Result = response;
            return Ok(_response);
        }

        [HttpGet]
        [Route("getVacancyById/{id}")]
        public async Task<IActionResult> GetVacancyById([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                var result = await _vacancyRepository.GetByIdAsync(id);
                var response = _mapper.Map<VacancyResponseDto>(result);
                _response.Result = response;
            }

            return Ok(_response);
        }

        [HttpGet]
        [Route("getByCompany/{email}")]
        public async Task<IActionResult> GetVacancyByCompany([FromRoute] string email)
        {
            if (email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                var employerDetails = await _companyRepository.GetByEmailAsync(email);

                if(employerDetails == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Employer Details Not Found is Empty";
                }
                else
                {
                    var organizationName = employerDetails.Organization;
            
                    var result = await _vacancyRepository.GetByNameAsync(organizationName);
                    List<VacancyResponseDto> vacancies = [];

                    foreach(var item in result)
                    {
                        vacancies.Add(_mapper.Map<VacancyResponseDto>(item));
                    }
                    _response.Result = vacancies;
                }
            }

            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        [Route("addVacancy")]
        public async Task<IActionResult> AddVacancy([FromBody] VacancyRequestDto request)
        {
            string email = request.PublishedBy;
            if(email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                var employerDetails = await _companyRepository.GetByEmailAsync(email);
                if(employerDetails == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please Enter Company Information";
                }
                else
                {
                    request.PublishedBy = employerDetails.Organization;

                    if(request == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Request is Empty";
                    }
                    else
                    {
                        Vacancy vacancy = _mapper.Map<Vacancy>(request);
                        var result = await _vacancyRepository.CreateAsync(vacancy);
                        var response = _mapper.Map<VacancyResponseDto>(result);
                        _response.Result = vacancy;
                        _response.Message = "Vacancy Created Successfully";
                    }
                }
            }
            return Ok(_response);
        }

        [HttpPut]
        [Authorize(Roles = "Employer")]
        [Route("updateVacancy/{id}")]
        public async Task<IActionResult> UpdateVacancy([FromBody] VacancyRequestDto vacancyDto, [FromRoute] Guid id)
        {
            string email = vacancyDto.PublishedBy;
            if (email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                var employerDetails = await _companyRepository.GetByEmailAsync(email);
                vacancyDto.PublishedBy = employerDetails.Organization;
                if (vacancyDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Vacancy Details are Empty";
                }
                else
                {
                    Vacancy request = _mapper.Map<Vacancy>(vacancyDto);
                    request.Id = id;
                    var result = await _vacancyRepository.UpdateAsync(request);

                    VacancyResponseDto response = _mapper.Map<VacancyResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Vacnacy Updated Successfully";
                }
            }
            return Ok(_response);
        }

        [HttpDelete]
        [Authorize(Roles = "Employer")]
        [Route("deleteVacancy/{id}")]
        public async Task<IActionResult> DeleteVacancy(Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                Vacancy vacancy = await _vacancyRepository.GetByIdAsync(id);
                if(vacancy == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Vacancy Not Found";
                }
                else
                {
                    var result = await _vacancyRepository.DeleteAsync(vacancy);
                    var response = _mapper.Map<VacancyResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Vacancy Deleted Successfully";
                }
            }
            return Ok(_response);
        }
    }
}
