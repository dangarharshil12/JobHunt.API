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
        [Route("GetAllQualifications")]
        public async Task<IActionResult> GetAllQualifications()
        {
            List<Qualification> result = await _qualificationRepository.GetAllAsync();
            List<QualificationDto> response = [];

            foreach(var item in result)
            {
                response.Add(_mapper.Map<QualificationDto>(item));
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("addQualification")]
        public async Task<IActionResult> CreateQualification([FromBody] QualificationDto request)
        {
            if(request == null)
            {
                return BadRequest();
            }

            Qualification Qualification = _mapper.Map<Qualification>(request);

            var result = await _qualificationRepository.CreateAsync(Qualification);
            QualificationDto response = _mapper.Map<QualificationDto>(result);
            return Ok(response);
        }
    }
}
