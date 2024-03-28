using AutoMapper;
using Azure.Core;
using JobHunt.Services.EmployerAPI;
using JobHunt.Services.EmployerAPI.Controllers;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using JobHunt.Services.EmployerAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobHunt.Services.Employer.Tests
{
    [TestFixture]
    public class CompanyControllerTests
    {
        private CompanyController _companyController;
        private Mock<ICompanyRepository> _mockcompanyRespository;
        private Mock<IImageRepository> _mockimageRespository;
        private IMapper _mapper;
        private EmployerDto _employer;
        private EmployerDto _updatedEmployer;
        private EmployerAPI.Models.Employer employer;
        private EmployerAPI.Models.Employer updatedEmployer;

        public CompanyControllerTests()
        {
            _employer = new EmployerDto
            {
                Organization = "Green Solutions",
                OrganizationType = "Agriculture",
                CompanyEmail = "info@greensolutions.com",
                CompanyPhone = "9988776655",
                NoOfEmployees = 100,
                StartYear = 2020,
                About = "Random Description",
                CreatedBy = "demoemployer@email.com",
                ImageUrl = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg",
            };

            _updatedEmployer = new EmployerDto
            {
                Organization = "Green Solutions2",
                OrganizationType = "Agriculture2",
                CompanyEmail = "info@greensolutions.com2",
                CompanyPhone = "9988776654",
                NoOfEmployees = 102,
                StartYear = 2022,
                About = "Random Description2",
                CreatedBy = "demoemployer@email.com",
                ImageUrl = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg",
            };

            employer = new EmployerAPI.Models.Employer
            {
                Id = new Guid("59878AA6-BB5C-4A90-5A69-08DC39204FE5"),
                Organization = "Green Solutions",
                OrganizationType = "Agriculture",
                CompanyEmail = "info@greensolutions.com",
                CompanyPhone = "9988776655",
                NoOfEmployees = 100,
                StartYear = 2020,
                About = "Random Description",
                CreatedBy = "demoemployer@email.com",
                ImageUrl = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg",
            };

            updatedEmployer = new EmployerAPI.Models.Employer
            {
                Id = new Guid("59878AA6-BB5C-4A90-5A69-08DC39204FE5"),
                Organization = "Green Solutions2",
                OrganizationType = "Agriculture2",
                CompanyEmail = "info@greensolutions.com2",
                CompanyPhone = "9988776654",
                NoOfEmployees = 102,
                StartYear = 2022,
                About = "Random Description2",
                CreatedBy = "demoemployer@email.com",
                ImageUrl = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg",
            };
        }

        [SetUp]
        public void Setup()
        {
            _mockcompanyRespository = new Mock<ICompanyRepository>();
            _mockimageRespository = new Mock<IImageRepository>();
            _mapper = MappingConfig.RegisterMaps().CreateMapper();

            _companyController = new CompanyController(_mockcompanyRespository.Object, _mapper, _mockimageRespository.Object);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task GetCompanyByEmail_NullOrEmptyEmail_ReturnsFail(string email)
        {
            // Act
            var result = await _companyController.GetCompanyByEmail(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Email is Empty"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(email), Times.Never);
        }

        
        
        [Test]
        public async Task GetCompanyByEmail_OrganizationDoesNotExists_ReturnsFailure()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((EmployerAPI.Models.Employer)null);

            string email = "test@email.com";

            // Act
            var result = await _companyController.GetCompanyByEmail(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Organization Information Not Found"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(email), Times.Once);
        }

        
        [Test]
        public async Task GetCompanyByEmail_OrganizationExists_ReturnSuccess()
        {
            // Arrange 
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(employer);

            string email = "info@greensolutions.com";

            // Act
            var result = await _companyController.GetCompanyByEmail(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.Empty);

            var responseResult = response.Result as EmployerDto;
            Assert.That(responseResult, Is.Not.Null);

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(email), Times.Once);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task GetCompanyByName_NullOrEmptyName_ShouldFail(string email)
        {
            // Act
            var result = await _companyController.GetCompanyByName(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Organization Name is Empty"));

            _mockcompanyRespository.Verify(u => u.GetByNameAsync(email), Times.Never);
        }

        
        
        [Test]
        public async Task GetCompanyByName_OrganizationDoesNotExists_ShouldFail()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((EmployerAPI.Models.Employer)null);

            string email = "test@email.com";

            // Act
            var result = await _companyController.GetCompanyByName(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Organization Information Not Found"));

            _mockcompanyRespository.Verify(u => u.GetByNameAsync(email), Times.Once);
        }

        
        [Test]
        public async Task GetCompanyByName_OrganizationExists_ShouldSuccess()
        {
            // Arrange 
            _mockcompanyRespository.Setup(u => u.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(employer);

            string email = "info@greensolutions.com";

            // Act
            var result = await _companyController.GetCompanyByName(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.Empty);

            var responseResult = response.Result as EmployerDto;
            Assert.That(responseResult, Is.Not.Null);

            _mockcompanyRespository.Verify(u => u.GetByNameAsync(email), Times.Once);
        }

        [Test]
        public async Task CreateCompany_EmptyRequest_ShouldFail()
        {
            // Arrange
            EmployerDto request = null;

            // Act
            var result = await _companyController.CreateCompany(request);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Request is Empty"));

            _mockcompanyRespository.Verify(u => u.CreateAsync(_mapper.Map<EmployerAPI.Models.Employer>(request)), Times.Never);
            _mockcompanyRespository.Verify(u => u.GetByNameAsync(_employer.Organization), Times.Never);
        }

        [Test]
        public async Task CreateCompany_OrganizationExists_ShouldFail()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(employer);

            // Act
            var result = await _companyController.CreateCompany(_employer);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Organization Information Not Found"));

            _mockcompanyRespository.Verify(u => u.GetByNameAsync(_employer.Organization), Times.Once);
            _mockcompanyRespository.Verify(u => u.CreateAsync(_mapper.Map<EmployerAPI.Models.Employer>(_employer)), Times.Never);
        }

        [Test]
        public async Task CreateCompany_OrganizationDoesNotExists_ShouldSuccess()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByNameAsync(_employer.Organization))
                .ReturnsAsync((EmployerAPI.Models.Employer)null);

            _mockcompanyRespository.Setup(u => u.CreateAsync(It.IsAny<EmployerAPI.Models.Employer>()))
                .ReturnsAsync(employer);
            

            // Act
            var result = await _companyController.CreateCompany(_employer);

            // Arrange
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Organization Profile Created Successfully!"));

            var responseResult = response.Result as EmployerDto;
            Assert.That(responseResult, Is.Not.Null);

            _mockcompanyRespository.Verify(u => u.GetByNameAsync(_employer.Organization), Times.Once);
        }

        [Test]
        public async Task UpdateCompany_OrganizationDoesNotExists_ShouldFail()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((EmployerAPI.Models.Employer)null);

            // Act
            var result = await _companyController.UpdateCompany(_employer);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Organization Information Not Found"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(_employer.CreatedBy), Times.Once);
        }

        [Test]
        public async Task UpdateCompany_OrganizationExists_ShouldSuccess()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(employer);

            _mockcompanyRespository.Setup(u => u.UpdateAsync(It.IsAny<EmployerAPI.Models.Employer>()))
                .ReturnsAsync(updatedEmployer);

            // Act
            var result = await _companyController.UpdateCompany(_updatedEmployer);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Organization Information Updated Successfully"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(_employer.CreatedBy), Times.Once);
        }

        [Test]
        public async Task UploadImage_ValidFile_ShouldReturnOk()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.FileName).Returns("2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg");
            formFile.Setup(f => f.Length).Returns(1024);

            _mockimageRespository.Setup(r => r.Upload(It.IsAny<IFormFile>(), It.IsAny<CompanyLogoDTO>()))
                .ReturnsAsync(new CompanyLogoDTO { Url = "https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg" });

            // Act
            var result = await _companyController.UploadImage(formFile.Object, "2d967414-5688-4dc0-a23a-dbf7880b19d6");

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.EqualTo("https://localhost:7284/Images/2d967414-5688-4dc0-a23a-dbf7880b19d6.jpg"));
            Assert.That(response.Message, Is.EqualTo("Image Upload Success!"));
        }

        [Test]
        public async Task UploadImage_InvalidFileExtension_ShouldReturnBadRequest()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.FileName).Returns("test.txt"); // Invalid file extension
            formFile.Setup(f => f.Length).Returns(1024); // File size less than 10MB

            // Act
            var result = await _companyController.UploadImage(formFile.Object, "test");

            // Assert
            var Result = result as ObjectResult;
            Assert.That(Result, Is.Not.Null);

            var response = Result.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Unsupported File Format"));
        }

        [Test]
        public async Task UploadImage_FileSizeExceedsLimit_ShouldReturnBadRequest()
        {
            // Arrange
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.FileName).Returns("test.jpg");
            formFile.Setup(f => f.Length).Returns(15 * 1024 * 1024); // File size exceeds 10MB

            // Act
            var result = await _companyController.UploadImage(formFile.Object, "test");

            // Assert
            var Result = result as ObjectResult;
            Assert.That(Result, Is.Not.Null);

            var response = Result.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("File Size cannot be more than 10MB"));
        }
    }
}
