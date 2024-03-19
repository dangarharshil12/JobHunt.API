using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using JobHunt.Services.JobSeekerAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/experience")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExperienceRepository _experienceRepository;
        protected ResponseDto _response;

        public ExperienceController(IMapper mapper, IExperienceRepository experienceRepository)
        {
            _mapper = mapper;
            _experienceRepository = experienceRepository;
            _response = new();
        }

        [HttpGet]
        [Route("getAllExperiencesByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllExperiencesByUserId(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is Empty";
            }
            else
            {
                List<UserExperience> result = await _experienceRepository.GetAllByUserIdAsync(userId);
                List<UserExperienceResponseDto> response = [];

                foreach(var item in result)
                {
                    response.Add(_mapper.Map<UserExperienceResponseDto>(item));
                }
                _response.Result = response;
            }
            return Ok(_response);
        }

        [HttpGet]
        [Route("getExperienceById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetExperienceById(Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                var result = await _experienceRepository.GetByIdAsync(id);

                if(result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No Experiences Found";
                }
                else
                {
                    var response = _mapper.Map<UserExperienceResponseDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("addExperience")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        public async Task<IActionResult> AddExperience([FromBody] UserExperienceRequestDto request)
        {
            if(request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Request is Empty";
            }
            else
            {
                UserExperience experience = _mapper.Map<UserExperience>(request);
                var result = await _experienceRepository.CreateAsync(experience);

                var response = _mapper.Map<UserExperienceResponseDto>(result);
                _response.Result = response;
                _response.Message = "Experience Added Successfully";
            }
            return Ok(_response);
        }

        [HttpPut]
        [Route("updateExperience/{id}")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        public async Task<IActionResult> UpdateExperience([FromBody] UserExperienceRequestDto request, [FromRoute] Guid id)
        {
            if(id == Guid.Empty || request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Id or Request is Empty";
            }
            else
            {
                UserExperience experience = _mapper.Map<UserExperience>(request);
                experience.Id = id;

                var result = await _experienceRepository.UpdateAsync(experience);
                if (result != null)
                {
                    UserExperienceResponseDto response = _mapper.Map<UserExperienceResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Experience Updated Successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Not Found";
                }
            }
            return Ok(_response);
        }

        [HttpDelete]
        [Route("deleteExperience/{id}")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        public async Task<IActionResult> DeleteExperience([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                UserExperience experience = await _experienceRepository.GetByIdAsync(id);
                if(experience == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Experience Not Found";
                }
                else
                {
                    var result = await _experienceRepository.DeleteAsync(experience);
                    UserExperienceResponseDto response = _mapper.Map<UserExperienceResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Experience Deleted Successfully";
                }
            }
            return Ok(_response);
        }
    }
}
