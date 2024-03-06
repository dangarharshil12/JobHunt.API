using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/jobSeeker")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;
        private readonly IResumeRepository _resumeRepository;
        protected ResponseDto _response;

        public ProfileController(IMapper mapper, IProfileRepository profileRepository, IResumeRepository resumeRepository)
        {
            _mapper = mapper;
            _profileRepository = profileRepository;
            _resumeRepository = resumeRepository;
            _response = new();
        }

        [HttpPost]
        [Route("getUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers([FromBody] List<Guid> userList)
        {
            List<User> users = await _profileRepository.GetUsersAsync(userList);
            var response = new List<UserDto>();
            foreach (var user in users)
            {
                response.Add(_mapper.Map<UserDto>(user));
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("getByEmail/{email}")]
        [Authorize]
        public async Task<IActionResult> GetProfileByEmail([FromRoute] string email) 
        {
            if(email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                User result = await _profileRepository.GetByEmailAsync(email);

                if(result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Profile not found";
                }
                else
                {
                    UserDto response = _mapper.Map<UserDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        [HttpGet]
        [Route("getByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetProfileByUserId([FromRoute] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "UserId is Empty";
            }
            else
            {
                User result = await _profileRepository.GetByUserIdAsync(userId);

                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Profile not found";
                }
                else
                {
                    UserDto response = _mapper.Map<UserDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("addProfile")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> AddProfile(UserDto user)
        {
            if(user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User is Empty";
            }
            else
            {
                User request = _mapper.Map<User>(user);
                var result = await _profileRepository.CreateAsync(request);
                UserDto response = _mapper.Map<UserDto>(result);

                _response.Result = response;
                _response.Message = "User Profile Added Successfully";
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("uploadResume")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile file)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var resume = new ResumeDto
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                };

                resume = await _resumeRepository.Upload(file, resume);
                _response.Result = resume.Url;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Resume Upload Model is not Valid";
            }
            return Ok(_response);
        }

        [HttpPut]
        [Route("updateProfile/{email}")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto user, [FromRoute] string email)
        {
            if(user == null || email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "User or Email is Empty";
            }
            else
            {
                User request = _mapper.Map<User>(user);

                var result = await _profileRepository.UpdateAsync(request);
                if(result != null)
                {
                    UserDto response = _mapper.Map<UserDto>(result);
                    _response.Result = response;
                    _response.Message = "User Profile Added Successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = " User Profile Update Failed";
                }
            }
            return Ok(_response);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".pdf", ".doc" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 5MB");
            }
        }
    }
}
