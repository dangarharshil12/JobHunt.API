using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/experience")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExperienceRepository _experienceRepository;
        public ExperienceController(IMapper mapper, IExperienceRepository experienceRepository)
        {
            _mapper = mapper;
            _experienceRepository = experienceRepository;
        }

        [HttpGet]
        [Route("getExperienceById/{id}")]
        public async Task<IActionResult> GetExperienceById(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _experienceRepository.GetByIdAsync(id);

            if(result == null)
            {
                return NotFound();
            }
            var response = _mapper.Map<UserExperienceResponseDto>(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("getAllExperiencesByUserId/{userId}")]
        public async Task<IActionResult> GetAllExperiencesByUserId(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest();
            }
            List<UserExperience> result = await _experienceRepository.GetAllByUserIdAsync(userId);
            List<UserExperienceResponseDto> response = [];

            foreach(var item in result)
            {
                response.Add(_mapper.Map<UserExperienceResponseDto>(item));
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("addExperience")]
        public async Task<IActionResult> AddExperience([FromBody] UserExperienceRequestDto request)
        {
            if(request == null)
            {
                return BadRequest();
            }
            UserExperience experience = _mapper.Map<UserExperience>(request);
            var result = await _experienceRepository.CreateAsync(experience);

            var response = _mapper.Map<UserExperienceResponseDto>(result);
            return Ok(response);
        }
    }
}
