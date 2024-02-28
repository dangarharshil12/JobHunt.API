using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/[controller]")]
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
    }
}
