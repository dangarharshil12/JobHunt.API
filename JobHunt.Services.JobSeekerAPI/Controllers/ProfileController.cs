using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/jobSeeker")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IMapper mapper, IProfileRepository profileRepository)
        {
            _mapper = mapper;
            _profileRepository = profileRepository;
        }

        [HttpGet]
        [Route("getByEmail/{email}")]
        public async Task<IActionResult> GetProfileByEmail([FromRoute] string email) 
        {
            if(email == null)
            {
                return BadRequest();
            }
            User result = await _profileRepository.GetByEmailAsync(email);

            if(result == null)
            {
                return Ok(null);
            }

            UserDto response = _mapper.Map<UserDto>(result);
            return Ok(response);
        }

        [HttpPost]
        [Route("addProfile")]
        public async Task<IActionResult> AddProfile(UserDto user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            User request = _mapper.Map<User>(user);

            var result = await _profileRepository.CreateAsync(request);
            UserDto response = _mapper.Map<UserDto>(result);

            return Ok(response);
        }

        [HttpPut]
        [Route("updateProfile/{email}")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto user, [FromRoute] string email)
        {
            if(user == null || email == null)
            {
                return BadRequest();
            }

            User request = _mapper.Map<User>(user);

            var result = await _profileRepository.UpdateAsync(request);
            if(result != null)
            {
                UserDto response = _mapper.Map<UserDto>(result);
                return Ok(response);
            }
            return NotFound();
        }
    }
}
