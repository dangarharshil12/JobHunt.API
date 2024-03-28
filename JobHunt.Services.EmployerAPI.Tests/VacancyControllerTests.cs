using AutoMapper;
using JobHunt.Services.EmployerAPI.Controllers;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

namespace JobHunt.Services.EmployerAPI.Tests
{
    [TestFixture]
    public class VacancyControllerTests
    {
        private VacancyController _vacancyController;
        private IMapper _mapper;
        private Mock<ICompanyRepository> _mockcompanyRespository;
        private Mock<IVacancyRepository> _mockvacancyRespository;
        private List<Vacancy> _vacancylist;
        private Vacancy _vacancy;
        private Vacancy _updatedVacancy;
        private VacancyRequestDto _newVacancyRequest;
        private VacancyRequestDto _updatedVacancyRequest;
        private Models.Employer _employer;

        public VacancyControllerTests()
        {
            _vacancy = new Vacancy()
            {
                Id = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                PublishedBy = "Green Solutions",
                PublishedDate = new DateTime(2024, 03, 05),
                NoOfVacancies = 2,
                MinimumQualification = "BE in Civil",
                JobTitle = "Site Manager",
                JobDescription = "job description for site manager job",
                ExperienceRequired = "3 to 5 years",
                LastDate = new DateTime(2024, 03, 8),
                MinimumSalary = 50000,
                MaximumSalary = 30000,
            };

            _newVacancyRequest = new VacancyRequestDto()
            {
                PublishedBy = "Green Solutions",
                PublishedDate = new DateTime(2024, 03, 05),
                NoOfVacancies = 2,
                MinimumQualification = "BE in Civil",
                JobTitle = "Site Manager",
                JobDescription = "job description for site manager job",
                ExperienceRequired = "3 to 5 years",
                LastDate = new DateTime(2024, 03, 8),
                MinimumSalary = 50000,
                MaximumSalary = 30000,
            };

            _updatedVacancyRequest = new VacancyRequestDto()
            {
                PublishedBy = "Green Solutions",
                PublishedDate = new DateTime(2024, 03, 05),
                NoOfVacancies = 5, // Changed
                MinimumQualification = "BE in Civil",
                JobTitle = "Site Manager",
                JobDescription = "job description for site manager job",
                ExperienceRequired = "3 to 8 years", // Changed
                LastDate = new DateTime(2024, 03, 8),
                MinimumSalary = 30000, // Changed
                MaximumSalary = 50000, // Changed
            };

            _updatedVacancy = new Vacancy()
            {
                Id = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                PublishedBy = "Green Solutions",
                PublishedDate = new DateTime(2024, 03, 05),
                NoOfVacancies = 5,
                MinimumQualification = "BE in Civil",
                JobTitle = "Site Manager",
                JobDescription = "job description for site manager job",
                ExperienceRequired = "3 to 8 years",
                LastDate = new DateTime(2024, 03, 8),
                MinimumSalary = 30000,
                MaximumSalary = 50000,
            };

            _vacancylist = new List<Vacancy>([])
            {
                new Vacancy()
                {
                    Id = new Guid("674da545-6231-476e-e018-08dc3c4d373a"),
                    PublishedBy= "Medi Solutions",
                    PublishedDate= new DateTime(2024, 03, 04),
                    NoOfVacancies= 2,
                    MinimumQualification= "B. Pharm",
                    JobTitle= "Quality Manager",
                    JobDescription= "job description for quality manager job",
                    ExperienceRequired= "3 to 5 years",
                    LastDate= new DateTime(2024, 03, 27),
                    MinimumSalary= 30000,
                    MaximumSalary= 37000,
                },
                new Vacancy()
                {
                    Id = new Guid("3660948d-570c-488b-4458-08dc3ccc67b1"),
                    PublishedBy= "Green Solutions",
                    PublishedDate= new DateTime(2024, 03, 05),
                    NoOfVacancies= 5,
                    MinimumQualification= "MBA",
                    JobTitle= "Senior Customer Executive",
                    JobDescription= "job description for Senior Customer Executive job",
                    ExperienceRequired= "3 to 5 years",
                    LastDate= new DateTime(2024, 03, 10),
                    MinimumSalary= 25000,
                    MaximumSalary= 30000,
                }
            };

            _employer = new Models.Employer
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
        }

        [SetUp]
        public void Setup()
        {
            _mockcompanyRespository = new Mock<ICompanyRepository>();
            _mockvacancyRespository = new Mock<IVacancyRepository>();
            _mapper = MappingConfig.RegisterMaps().CreateMapper();

            _vacancyController = new VacancyController(_mapper, _mockvacancyRespository.Object, _mockcompanyRespository.Object);
        }

        [Test]
        public async Task GetAllVacancies_ReturnVacancyList()
        {
            // Arrange
            _mockvacancyRespository.Setup(u => u.GetAllAsync()).ReturnsAsync(_vacancylist);

            // Act
            var result = await _vacancyController.GetAllVacancies();

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.Not.Empty);
            Assert.That(response.Result, Is.InstanceOf<List<VacancyResponseDto>>());

            _mockvacancyRespository.Verify(u => u.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetVacancyById_EmptyVacancyId_ReturnFailure()
        {
            // Arrange
            Guid userId = Guid.Empty;

            // Act
            var result = await _vacancyController.GetVacancyById(userId);

            // Assert
            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Id is Empty"));

            _mockvacancyRespository.Verify(u => u.GetByIdAsync(userId), Times.Never);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetVacancyById_ValidUserId_ReturnSuccess(bool hasApplied)
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim("Id", Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _vacancyController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            Guid vacancyId = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7");
            _mockvacancyRespository.Setup(r => r.CheckApplicationAsync(It.IsAny<Guid>(), vacancyId))
                .ReturnsAsync(hasApplied);

            _mockvacancyRespository.Setup(r => r.GetByIdAsync(vacancyId))
                .ReturnsAsync(_vacancy);

            // Act
            var result = await _vacancyController.GetVacancyById(vacancyId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.InstanceOf<VacancyResponseDto>());

            var responseResult = response.Result as VacancyResponseDto;
            Assert.That(responseResult.Applied, Is.EqualTo(hasApplied));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task GetAllVacanciesByCompany_NullOrEmptyEmail_ReturnFailure(string email)
        {
            // Act
            var result = await _vacancyController.GetAllVacanciesByCompany(email);

            // Assert
            var Result = result as ObjectResult;
            Assert.That(Result, Is.Not.Null);

            var response = Result.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Email is Empty"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(email), Times.Never);
        }

        [Test]
        public async Task GetVacancyByCompany_OrganizationDoesNotExists_ReturnFailure()
        {
            // Arrange
            string email = "organizationdoesnotexists@email.com";
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Models.Employer)null);

            // Act
            var result = await _vacancyController.GetAllVacanciesByCompany(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Employer Details Not Found"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(email), Times.Once);
        }

        [Test]
        public async Task GetAllVacanciesByCompany_OrganizationExists_ReturnsSuccess()
        {
            // Arrange
            var email = "demoemployer@email.com";
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_employer);

            _mockvacancyRespository.Setup(u => u.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(_vacancylist);

            // Act
            var result = await _vacancyController.GetAllVacanciesByCompany(email);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Result, Is.Not.Empty);
            Assert.That(response.Result, Is.InstanceOf<List<VacancyResponseDto>>());

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(email), Times.Once);
            _mockvacancyRespository.Verify(u => u.GetByNameAsync(_employer.Organization), Times.Once);
        }

        [Test]
        public async Task AddVacancy_EmptyRequest_ReturnFailure()
        {
            // Act
            var result = await _vacancyController.AddVacancy(null);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Vacancy Request is Empty"));
        }

        [Test]
        public async Task AddVacnacy_OrganizationDoesNotExists_ReturnFailure()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Models.Employer)null);

            // Act
            var result = await _vacancyController.AddVacancy(_newVacancyRequest);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Please Enter Organization Information"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(_newVacancyRequest.PublishedBy), Times.Once);
        }

        [Test]
        public async Task AddVacnacy_OrganizationExists_ReturnSuccess()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_employer);

            _mockvacancyRespository.Setup(u => u.CreateAsync(It.IsAny<Vacancy>()))
                .ReturnsAsync(_vacancy);

            // Act
            var result = await _vacancyController.AddVacancy(_newVacancyRequest);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Vacancy Created Successfully"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(_newVacancyRequest.PublishedBy), Times.Once);
        }

        [Test]
        public async Task UpdateVacancy_EmptyVacancyRequest_ReturnFailure()
        {
            // Act
            var result = await _vacancyController.UpdateVacancy(null, new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"));

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Vacancy Details are Empty"));
        }

        [Test]
        public async Task UpdateVacancy_VacancyRequest_ReturnsSuccess()
        {
            // Arrange
            _mockcompanyRespository.Setup(u => u.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_employer);

            _mockvacancyRespository.Setup(u => u.UpdateAsync(It.IsAny<Vacancy>()))
                .ReturnsAsync(_updatedVacancy);

            // Act
            var result = await _vacancyController.UpdateVacancy(_updatedVacancyRequest, new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"));

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Vacnacy Updated Successfully"));

            _mockcompanyRespository.Verify(u => u.GetByEmailAsync(_newVacancyRequest.PublishedBy), Times.Once);
        }

        [Test]
        public async Task DeleteVacancy_EmptyId_ReturnsFailure()
        {
            // Act
            var result = await _vacancyController.DeleteVacancy(Guid.Empty);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Id is Empty"));
        }

        [Test]
        public async Task DeleteVacancy_VacancyDoesNotExists_ReturnFailure()
        {
            // Arrange
            _mockvacancyRespository.Setup(u => u.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Vacancy)null);

            // Act
            var result = await _vacancyController.DeleteVacancy(new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"));

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Vacancy Not Found"));

            _mockvacancyRespository.Verify(u => u.GetByIdAsync(new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7")), Times.Once);
        }

        [Test]
        public async Task DeleteVacancy_VacancyExists_ReturnsSuccess()
        {
            // Arrange
            _mockvacancyRespository.Setup(u => u.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_vacancy);

            _mockvacancyRespository.Setup(u => u.DeleteAsync(It.IsAny<Vacancy>()))
                .ReturnsAsync(_vacancy);

            // Act
            var result = await _vacancyController.DeleteVacancy(new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"));

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Vacancy Deleted Successfully"));

            _mockvacancyRespository.Verify(u => u.GetByIdAsync(new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7")), Times.Once);
            _mockvacancyRespository.Verify(u => u.DeleteAsync(_vacancy), Times.Once);
        }
    }
}
