using AutoMapper;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.JobSeekerAPI.Controllers
{
    [Route("api/qualification")]
    [ApiController]
    public class QualificationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQualificationRepository _qualificationRepository;

        public QualificationController(IQualificationRepository qualificationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _qualificationRepository = qualificationRepository;
        }

        [HttpGet]
        [Route("GetAllQualificationsByUserId/{id}")]
        public async Task<IActionResult> GetAllQualifications(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            List<Qualification> result = await _qualificationRepository.GetAllByUserIdAsync(id);
            List<QualificationResponseDto> response = [];

            foreach(var item in result)
            {
                response.Add(_mapper.Map<QualificationResponseDto>(item));
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetQualificationById/{id}")]
        public async Task<IActionResult> GetQualificationById(Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _qualificationRepository.GetByIdAsync(id);
            if(result == null)
            {
                return Ok(null);
            }
            QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
            return Ok(response);
        }

        [HttpPost]
        [Route("addQualification")]
        public async Task<IActionResult> CreateQualification([FromBody] QualificationRequestDto request)
        {
            if(request == null)
            {
                return BadRequest();
            }

            Qualification Qualification = _mapper.Map<Qualification>(request);

            var result = await _qualificationRepository.CreateAsync(Qualification);
            QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
            return Ok(response);
        }

        [HttpPut]
        [Route("updateQualification/{id}")]
        public async Task<IActionResult> UpdateQualification([FromBody] QualificationRequestDto request, [FromRoute] Guid id)
        {
            if (request == null || id == Guid.Empty)
            {
                return BadRequest();
            }
            Qualification qualification = _mapper.Map<Qualification>(request);
            qualification.Id = id;

            var result = await _qualificationRepository.UpdateAsync(qualification);
            if (result != null)
            {
                QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
                return Ok(response);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("deleteQualification/{id}")]
        public async Task<IActionResult> DeleteQualification([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            Qualification qualification = await _qualificationRepository.GetByIdAsync(id);
            if(qualification == null)
            {
                return NotFound();
            }
            var result = await _qualificationRepository.DeleteAsync(qualification);
            QualificationResponseDto response = _mapper.Map<QualificationResponseDto>(result);
            return Ok(response);
        }
    }
}
