
using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.EmployerAPI.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> GetCompanyByEmail([FromRoute] string email)
        {
            if(email == null)
            {
                return BadRequest();
            }
            var result = await _companyRepository.GetByEmailAsync(email);
            if (result == null)
            {
                return Ok(null);
            }

            var response = _mapper.Map<EmployerDto>(result);
            return Ok(response);
        }

        [HttpGet]
        [Route("getProfileByName/{name}")]
        public async Task<IActionResult> GetCompanyByName([FromRoute] string name)
        {
            if (name == null)
            {
                return BadRequest();
            }
            var result = await _companyRepository.GetByNameAsync(name);
            if (result == null)
            {
                return Ok(null);
            }

            var response = _mapper.Map<EmployerDto>(result);
            return Ok(response);
        }

        [HttpPost]
        [Route("addDetails")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateCompany([FromBody] EmployerDto request)
        {
            if(request == null)
            {
                return BadRequest(request);
            }

            Employer employer = _mapper.Map<Employer>(request);
            var result = await _companyRepository.CreateAsync(employer);
            EmployerDto response = _mapper.Map<EmployerDto>(result);

            return Ok(result);
        }

        [HttpPut]
        [Route("updateDetails/{email}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateCompany([FromRoute] string email, [FromBody] EmployerDto request)
        {
            if (email == null)
            {
                return BadRequest();
            }
            var result = await _companyRepository.GetByEmailAsync(email);
            if (result == null)
            {
                return NotFound(null);
            }
            Employer employer = _mapper.Map<Employer>(request);
            employer.Id = result.Id;

            result = await _companyRepository.UpdateAsync(employer);
            if(result == null)
            {
                return BadRequest();
            }
            EmployerDto response = _mapper.Map<EmployerDto>(result);
            return Ok(response);
        }
    }
}
