﻿
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
        protected ResponseDto _response;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IActionResult> GetCompanyByEmail([FromRoute] string email)
        {
            if(email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                var result = await _companyRepository.GetByEmailAsync(email);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Organization Information Not Found";
                }
                else
                {
                    var response = _mapper.Map<EmployerDto>(result);
                    _response.Result = response;
                }
            }

            return Ok(_response);
        }

        [HttpGet]
        [Route("getProfileByName/{name}")]
        public async Task<IActionResult> GetCompanyByName([FromRoute] string name)
        {
            if (name == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Organization Name is Empty";
            }
            else
            {
                var result = await _companyRepository.GetByNameAsync(name);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Organization Information Not Found";
                }
                else
                {
                    var response = _mapper.Map<EmployerDto>(result);
                    _response.Result= response;
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("addDetails")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateCompany([FromBody] EmployerDto request)
        {
            if(request == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Request is Empty";
            }
            else
            {
                var existingCompany = await _companyRepository.GetByNameAsync(request.Organization);
                if (existingCompany != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Organization Information Not Found";
                }
                else
                {
                    Employer employer = _mapper.Map<Employer>(request);
                    var result = await _companyRepository.CreateAsync(employer);
                    EmployerDto response = _mapper.Map<EmployerDto>(result);
                    _response.Message = "Organization Information Created Successfully!";
                    _response.Result= response;
                }
            }
            return Ok(_response);
        }

        [HttpPut]
        [Route("updateDetails/{email}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateCompany([FromRoute] string email, [FromBody] EmployerDto request)
        {
            if (email == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Email is Empty";
            }
            else
            {
                var result = await _companyRepository.GetByEmailAsync(email);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Organization information not found";
                }
                else
                {
                    Employer employer = _mapper.Map<Employer>(request);
                    employer.Id = result.Id;

                    result = await _companyRepository.UpdateAsync(employer);
                    if(result == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Update Failed";
                    }
                    else
                    {
                        EmployerDto response = _mapper.Map<EmployerDto>(result);
                        _response.Message = "Organization Information Updated Successfully";
                        _response.Result = response;
                    }
                }
            }
            return Ok(_response);
        }
    }
}
