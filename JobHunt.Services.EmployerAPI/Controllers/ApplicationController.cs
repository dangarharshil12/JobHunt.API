using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using JobHunt.Services.EmployerAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.EmployerAPI.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public ApplicationController(IApplicationRepository applicationRepository, IMapper mapper, IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [Route("getAllByUser/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> getApplicationsByUserId([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                var result = await _applicationRepository.GetAllByUserIdAsync(id);
                List<UserVacancyResponseDto> response = [];
                foreach (var application in result)
                {
                    response.Add(_mapper.Map<UserVacancyResponseDto>(application));
                }
                _response.Result = response;
            }
            return Ok(_response);
        }

        [HttpGet]
        [Route("getAllByVacancy/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> getApplicationsByVacancyId([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                List<UserVacancyRequest> result = await _applicationRepository.GetAllByVacancyIdAsync(id);

                List<UserVacancyResponseDto> response = [];
                List<Guid> usersList = [];

                foreach (var application in result)
                {
                    usersList.Add(application.UserId);
                    response.Add(_mapper.Map<UserVacancyResponseDto>(application));
                }

                List<UserDto> users = await _profileRepository.GetUsers(usersList);
                foreach (var item in response)
                {
                    item.User = users.FirstOrDefault(u => u.Id == item.UserId);
                }
                _response.Result = response;
            }

            return Ok(_response);
        }

        [HttpPost]
        [Route("createApplication")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> createApplication([FromBody] UserVacancyRequestDto request)
        {
            if (request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Request is Empty";
            }
            else
            {
                UserVacancyRequest userVacancyRequest = _mapper.Map<UserVacancyRequest>(request);
                var existingVacancy = await _applicationRepository.GetDetailAsync(userVacancyRequest.UserId, userVacancyRequest.VacancyId);
                if(existingVacancy != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "You have Already Applied";
                }
                else
                {
                    userVacancyRequest.ApplicationStatus = "SUBMITTED";
                    var result = await _applicationRepository.CreateAsync(userVacancyRequest);
                    var response = _mapper.Map<UserVacancyResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Applied Successfully";
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("processApplication")]
        [Authorize(Roles = SD.RoleEmployer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> processApplication([FromBody] statusChangeRequestDto request)
        {
            string status = request.status;
            var application = await _applicationRepository.GetDetailByIdAsync(request.id);

            if(application == null) 
            { 
                _response.IsSuccess = false;
                _response.Message = "Job Application Not Found";
            }
            else
            {
                if(status == "")
                {
                    _response.IsSuccess = false;
                    _response.Message = "New Status is not Provided.";
                }
                else
                {
                    application.ApplicationStatus = status;
                    var result = await _applicationRepository.UpdateAsync(application);
                    if(result != null)
                    {
                        _response.Result = _mapper.Map<UserVacancyResponseDto>(result);
                        _response.Message = "Status Updated Successfully";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Status Update Failed";
                    }
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("paginationEndpoint")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> pagination([FromBody] SP_VacancyRequestDto request)
        {
            if(request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Request is Empty";
            }
            else
            {
                List<UserVacancyRequest> result = _applicationRepository.GetAllVacancyByPageAsync(request);
                List<UserVacancyResponseDto> response = [];

                List<Guid> usersList = [];

                foreach (var application in result)
                {
                    usersList.Add(application.UserId);
                    response.Add(_mapper.Map<UserVacancyResponseDto>(application));
                }

                List<UserDto> users = await _profileRepository.GetUsers(usersList);
                foreach (var item in response)
                {
                    item.User = users.FirstOrDefault(u => u.Id == item.UserId);
                }

                _response.Result = new {
                    totalRecords= response.FirstOrDefault()?.TotalRecords,
                    results = response
                };
            }
            return Ok(_response);
        }
    }
}
