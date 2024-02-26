
using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        [Route("addDetails")]
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
    }
}
