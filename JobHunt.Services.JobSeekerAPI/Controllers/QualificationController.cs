using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using JobHunt.Services.JobSeekerAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/qualification")]
    [ApiController]
    public class QualificationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQualificationRepository _qualificationRepository;
        protected ResponseDto _response;

        public QualificationController(IQualificationRepository qualificationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
            _response = new();
        }

        [HttpGet]
        [Authorize]
        [Route("GetAllQualificationsByUserId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllQualifications(Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                List<Qualification> result = await _qualificationRepository.GetAllByUserIdAsync(id);
                List<QualificationResponseDto> response = [];

                foreach(var item in result)
                {
                    response.Add(_mapper.Map<QualificationResponseDto>(item));
                }
                _response.Result = response;
            }
            return Ok(_response);
        }

        [HttpGet]
        [Authorize]
        [Route("GetQualificationById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetQualificationById(Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                var result = await _qualificationRepository.GetByIdAsync(id);
                if(result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Qualification Not Found";
                }
                else
                {
                    QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
                    _response.Result = response;
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("addQualification")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateQualification([FromBody] QualificationRequestDto request)
        {
            if(request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Request is Empty";
            }
            else
            {
                Qualification Qualification = _mapper.Map<Qualification>(request);
                var result = await _qualificationRepository.CreateAsync(Qualification);
                QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);

                _response.Result = response;
                _response.Message = "Qualification Added Successfully";
            }
            return Ok(_response);
        }

        [HttpPut]
        [Route("qualification/{id}")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        public async Task<IActionResult> UpdateQualification([FromBody] QualificationRequestDto request, [FromRoute] Guid id)
        {
            if (request == null || id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Request or Id is Empty";
            }
            else
            {
                Qualification qualification = _mapper.Map<Qualification>(request);
                qualification.Id = id;

                var result = await _qualificationRepository.UpdateAsync(qualification);
                if (result != null)
                {
                    QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Qualification Updated Successfully";
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Update Failed";
                }
            }
            return Ok(_response);
        }

        [HttpDelete]
        [Route("qualification/{id}")]
        [Authorize(Roles = SD.RoleJobSeeker)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteQualification([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                _response.IsSuccess = false;
                _response.Message = "Id is Empty";
            }
            else
            {
                Qualification qualification = await _qualificationRepository.GetByIdAsync(id);
                if(qualification == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Qualification Not Found";
                }
                else
                {
                    var result = await _qualificationRepository.DeleteAsync(qualification);
                    QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
                    _response.Result = response;
                    _response.Message = "Qualification Deleted Successfully";
                }
            }
            return Ok(_response);
        }
    }
}
