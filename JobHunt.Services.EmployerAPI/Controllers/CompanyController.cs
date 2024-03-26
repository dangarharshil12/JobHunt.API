
using AutoMapper;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using JobHunt.Services.EmployerAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.EmployerAPI.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        protected ResponseDto _response;

        public CompanyController(ICompanyRepository companyRepository, IMapper mapper, IImageRepository imageRepository)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _response = new();
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [Route("companyDetails")]
        [Authorize(Roles = SD.RoleEmployer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [Route("companyDetails")]
        [Authorize(Roles = SD.RoleEmployer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCompany([FromBody] EmployerDto request)
        {
                var result = await _companyRepository.GetByEmailAsync(request.CreatedBy);
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
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleEmployer)]
        [Route("uploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var resume = new CompanyLogoDTO
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName
                };

                resume = await _imageRepository.Upload(file, resume);
                _response.Result = resume.Url;
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Image Upload Model is not Valid";
            }
            return Ok(_response);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File Size cannot be more than 10MB");
            }
        }
    }
}
