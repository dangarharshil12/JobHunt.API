using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.EmployerAPI.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getAllByUser/{id}")]
        public async Task<IActionResult> getApplicationsByUserId([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _applicationRepository.GetAllByUserIdAsync(id);
            List<UserVacancyResponseDto> response = [];
            foreach (var application in result)
            {
                response.Add(_mapper.Map<UserVacancyResponseDto>(application));
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getAllByVacancy/{id}")]
        public async Task<IActionResult> getApplicationsByVacancyId([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _applicationRepository.GetAllByVacancyIdAsync(id);
            List<UserVacancyResponseDto> response = [];
            foreach (var application in result)
            {
                response.Add(_mapper.Map<UserVacancyResponseDto>(application));
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("createApplication")]
        public async Task<IActionResult> createApplication([FromBody] UserVacancyRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            UserVacancyRequest userVacancyRequest = _mapper.Map<UserVacancyRequest>(request);
            var existingVacancy = await _applicationRepository.GetDetailAsync(userVacancyRequest.UserId, userVacancyRequest.VacancyId);
            if(existingVacancy != null)
            {
                return Ok("Already Applied");
            }
            var result = await _applicationRepository.CreateAsync(userVacancyRequest);
            var response = _mapper.Map<UserVacancyResponseDto>(result);
            return Ok(response);
        }
    }
}
