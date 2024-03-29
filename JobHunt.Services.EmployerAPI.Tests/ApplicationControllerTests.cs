using AutoMapper;
using JobHunt.Services.EmployerAPI.Controllers;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using JobHunt.Services.EmployerAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace JobHunt.Services.EmployerAPI.Tests
{
    [TestFixture]
    public class ApplicationControllerTests
    {
        private ApplicationController _applicationController;
        private IMapper _mapper;
        private Mock<IApplicationRepository> _mockapplicaionRepository;
        private Mock<IProfileRepository> _mockprofileRepository;

        private List<UserVacancyRequest> _applicationlist;
        private List<UserVacancyRequest> _applicationlist2;
        private List<UserDto> _userlist;

        private SP_VacancyRequestDto _request;
        private SP_VacancyRequestDto _request1;
        private SP_VacancyRequestDto _request2;
        private SP_VacancyRequestDto _request3;
        private SP_VacancyRequestDto _request4;

        public ApplicationControllerTests()
        {
            _applicationlist = new List<UserVacancyRequest>([])
            {
                new UserVacancyRequest()
                {
                  Id =  new Guid("1a3d8c7e-db11-45fe-c2b6-08dc3c361c6e"),
                  VacancyId =  new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                  Vacancy =  new Vacancy() {
                    Id =  new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy =  "Green Solutions",
                    PublishedDate =  new DateTime(2024, 02, 29),
                    NoOfVacancies =  2,
                    MinimumQualification =  "BE in Civil",
                    JobTitle =  "Site Manager",
                    JobDescription =  "random description for site manager job",
                    ExperienceRequired =  "3 to 5 years",
                    LastDate =  new DateTime(2024, 03, 08),
                    MinimumSalary =  50000,
                    MaximumSalary =  30000
                  },
                  UserId =  new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                  AppliedDate =  new DateTime(2024, 03, 04),
                  ApplicationStatus =  "SUBMITTED",
                  TotalRecords = 0,
                  User = null
                },
                new UserVacancyRequest()
                {
                  Id = new Guid("c585e9b9-7028-4fca-4ae9-08dc4192f507"),
                  VacancyId = new Guid("51b1bbe3-14b5-4000-bbe7-08dc3ea06d85"),
                  Vacancy = new Vacancy() {
                    Id = new Guid("51b1bbe3-14b5-4000-bbe7-08dc3ea06d85"),
                    PublishedBy = "Medi Solutions",
                    PublishedDate =  new DateTime(2024, 03, 07),
                    NoOfVacancies =  5,
                    MinimumQualification =  "Graduate in any stream",
                    JobTitle =  "Marketing Intern",
                    JobDescription =  "random description for marketing interns",
                    ExperienceRequired =  "No Experience Required",
                    LastDate =  new DateTime(2024, 03, 27),
                    MinimumSalary =  10000,
                    MaximumSalary =  15000
                  },
                  UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                  AppliedDate =  new DateTime(2024, 03, 11),
                  ApplicationStatus =  "SUBMITTED",
                  TotalRecords = 0,
                  User = null
                },
                new UserVacancyRequest()
                {
                  Id = new Guid("a1a31c1e-9961-45c3-a58f-08dc4d70b9a5"),
                  VacancyId = new Guid("cf91f57a-cea8-4af6-58be-08dc4a3dde2d"),
                  Vacancy = new Vacancy(){
                    Id = new Guid("cf91f57a-cea8-4af6-58be-08dc4a3dde2d"),
                    PublishedBy =  "Shelby Foundation",
                    PublishedDate =  new DateTime(2024, 03, 22),
                    NoOfVacancies =  5,
                    MinimumQualification =  "Graduate in any stream",
                    JobTitle =  "Interns",
                    JobDescription =  "random description for Interns",
                    ExperienceRequired =  "No Experience Required",
                    LastDate =  new DateTime(2024, 04, 25),
                    MinimumSalary =  15000,
                    MaximumSalary =  10000
                  },
                  UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                  AppliedDate =  new DateTime(2024, 03, 26),
                  ApplicationStatus =  "SUBMITTED",
                  TotalRecords =  0,
                  User = null
                }
            };

            _applicationlist2 = new List<UserVacancyRequest>([])
            {
                new UserVacancyRequest()
                {
                  Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
                  VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                  Vacancy = new Vacancy() {
                    Id =  new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy =  "Green Solutions",
                    PublishedDate =  new DateTime(2024, 02, 29),
                    NoOfVacancies =  2,
                    MinimumQualification =  "BE in Civil",
                    JobTitle =  "Site Manager",
                    JobDescription =  "random description for site manager job",
                    ExperienceRequired =  "3 to 5 years",
                    LastDate =  new DateTime(2024, 03, 08),
                    MinimumSalary =  50000,
                    MaximumSalary =  30000
                  },
                  UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                  AppliedDate =  new DateTime(2024, 03, 11),
                  ApplicationStatus =  "SUBMITTED",
                  TotalRecords = 0,
                },
                new UserVacancyRequest()
                {
                  Id = new Guid("6C9C3DFF-ECE6-477B-76B8-08DC4739927A"),
                  VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                  Vacancy = new Vacancy(){
                    Id =  new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy =  "Green Solutions",
                    PublishedDate =  new DateTime(2024, 02, 29),
                    NoOfVacancies =  2,
                    MinimumQualification =  "BE in Civil",
                    JobTitle =  "Site Manager",
                    JobDescription =  "random description for site manager job",
                    ExperienceRequired =  "3 to 5 years",
                    LastDate =  new DateTime(2024, 03, 08),
                    MinimumSalary =  50000,
                    MaximumSalary =  30000
                  },
                  UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                  AppliedDate =  new DateTime(2024, 03, 26),
                  ApplicationStatus =  "SUBMITTED",
                  TotalRecords =  0,
                },
                new UserVacancyRequest()
                {
                  Id = new Guid("E1F2BCDE-797F-419A-630A-08DC4A461BBC"),
                  VacancyId = new Guid("cf91f57a-cea8-4af6-58be-08dc4a3dde2d"),
                  Vacancy = new Vacancy(){
                    Id =  new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy =  "Green Solutions",
                    PublishedDate =  new DateTime(2024, 02, 29),
                    NoOfVacancies =  2,
                    MinimumQualification =  "BE in Civil",
                    JobTitle =  "Site Manager",
                    JobDescription =  "random description for site manager job",
                    ExperienceRequired =  "3 to 5 years",
                    LastDate =  new DateTime(2024, 03, 08),
                    MinimumSalary =  50000,
                    MaximumSalary =  30000
                  },
                  UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                  AppliedDate =  new DateTime(2024, 03, 26),
                  ApplicationStatus =  "SUBMITTED",
                  TotalRecords =  0,
                }
            };

            _userlist = new List<UserDto>([])
            {
                new UserDto()
                {
                    Id = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                    FirstName = "First",
                    LastName = "Last",
                    Email = "demouser@email.com",
                    Phone = "9876543210",
                    Address = "House No: 1002,\nRandom Street,\nRandom City",
                    TotalExperience = 5,
                    ExpectedSalary = 70000,
                    ResumeUrl = "https://localhost:7246/Resumes/dfb764c1-0dea-45a0-b04f-e138c0e08c22.pdf",
                    ImageUrl = "https://localhost:7246/Images/dfb764c1-0dea-45a0-b04f-e138c0e08c22.jpg",
                    DateOfBirth = new DateTime(2000, 08, 07)
                },
                new UserDto()
                {
                    Id = new Guid("8edbd66b-3289-4535-90ee-77448716c03a"),
                    FirstName = "First2",
                    LastName = "Last2",
                    Email = "demouser2@email.com",
                    Phone = "9876543210",
                    Address = "House No: 1002,\nRandom Street,\nRandom City",
                    TotalExperience = 5,
                    ExpectedSalary = 45000,
                    ResumeUrl = "https://localhost:7246/Resumes/8edbd66b-3289-4535-90ee-77448716c03a.pdf",
                    ImageUrl = "",
                    DateOfBirth = new DateTime(2002, 02, 10)
                },
                new UserDto()
                {
                    Id = new Guid("41e68188-4d6c-49db-964a-b0c64ef4c57b"),
                    FirstName = "Kakashi",
                    LastName = "Hatake",
                    Email = "kakashithecopyninja@leaf.com",
                    Phone = "9876543210",
                    Address = "House No: 1002,\nRandom Street,\nRandom City",
                    TotalExperience = 10,
                    ExpectedSalary = 90000,
                    ResumeUrl = "https://localhost:7246/Resumes/41e68188-4d6c-49db-964a-b0c64ef4c57b.pdf",
                    ImageUrl = "",
                    DateOfBirth = new DateTime(1990, 01, 15)
                }
            };

            _request = new SP_VacancyRequestDto()
            {
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                PageSize = 3,
                StartIndex = 0
            };

            _request1 = new SP_VacancyRequestDto()
            {
                VacancyId = Guid.Empty,
                PageSize = 0,
                StartIndex = -1
            };

            _request2 = new SP_VacancyRequestDto()
            {
                VacancyId = Guid.Empty,
                PageSize = 3,
                StartIndex = 0
            };

            _request3 = new SP_VacancyRequestDto()
            {
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                PageSize = 0,
                StartIndex = 5
            };

            _request4 = new SP_VacancyRequestDto()
            {
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                PageSize = 2,
                StartIndex = -1
            };
        }

        [SetUp]
        public void Setup()
        {
            _mockapplicaionRepository = new Mock<IApplicationRepository>();
            _mockprofileRepository = new Mock<IProfileRepository>();
            _mapper = MappingConfig.RegisterMaps().CreateMapper();

            _applicationController = new ApplicationController(_mockapplicaionRepository.Object, _mapper, _mockprofileRepository.Object);
        }

        [Test]
        public async Task GetApplicationsByUserId_EmptyUserId_ReturnsFailure()
        {
            // Arrange
            Guid userId = Guid.Empty;

            // Act 
            var result = await _applicationController.GetApplicationsByUserId(userId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Id is Empty"));

            _mockapplicaionRepository.Verify(u => u.GetAllByUserIdAsync(userId), Times.Never);
        }

        [Test]
        public async Task GetApplicationsByUserId_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            Guid userId = new Guid("DFB764C1-0DEA-45A0-B04F-E138C0E08C22");

            _mockapplicaionRepository.Setup(u => u.GetAllByUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_applicationlist);

            // Act
            var result = await _applicationController.GetApplicationsByUserId(userId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.Empty);

            var responseResult = response.Result as List<UserVacancyResponseDto>;
            Assert.That(responseResult.Count, Is.EqualTo(3));

            _mockapplicaionRepository.Verify(u => u.GetAllByUserIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task GetApplicationsByVacancyId_EmptyUserId_ReturnsFailure()
        {
            // Arrange
            Guid vacancyId = Guid.Empty;

            // Act 
            var result = await _applicationController.GetApplicationsByVacancyId(vacancyId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Id is Empty"));

            _mockapplicaionRepository.Verify(u => u.GetAllByVacancyIdAsync(vacancyId), Times.Never);
        }

        [Test]
        public async Task ApplicationsByVacancyId_ValidUserId_ReturnsSuccess()
        {
            // Arrange
            Guid vacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7");

            _mockapplicaionRepository.Setup(u => u.GetAllByVacancyIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_applicationlist2);

            _mockprofileRepository.Setup(u => u.GetUsers(It.IsAny<List<Guid>>()))
                .ReturnsAsync(_userlist);

            // Act
            var result = await _applicationController.GetApplicationsByVacancyId(vacancyId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.Empty);

            var responseResult = response.Result as List<UserVacancyResponseDto>;
            Assert.That(responseResult.Count, Is.EqualTo(3));

            _mockapplicaionRepository.Verify(u => u.GetAllByVacancyIdAsync(vacancyId), Times.Once);
        }

        [Test]
        public async Task CreateApplication_EmptyRequest_ReturnsFailure()
        {
            // Act
            var result = await _applicationController.CreateApplication(null);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Request is Empty"));
        }

        [Test]
        public async Task CreateApplication_AlreadyApplied_ReturnsFailure()
        {
            // Arrange
            _mockapplicaionRepository.Setup(u => u.GetDetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new UserVacancyRequest { });

            // Act
            var result = await _applicationController.CreateApplication(new UserVacancyRequestDto() { });

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("You have Already Applied"));
        }

        [Test]
        public async Task CreateApplication_ApplyingFirstTime_ReturnsSuccess()
        {
            // Arrange
            var request = new UserVacancyRequestDto()
            {
                VacancyId = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                Vacancy = new Vacancy()
                {
                    Id = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy = "Green Solutions",
                    PublishedDate = new DateTime(2024, 02, 29),
                    NoOfVacancies = 2,
                    MinimumQualification = "BE in Civil",
                    JobTitle = "Site Manager",
                    JobDescription = "random description for site manager job",
                    ExperienceRequired = "3 to 5 years",
                    LastDate = new DateTime(2024, 03, 08),
                    MinimumSalary = 50000,
                    MaximumSalary = 30000
                },
                UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                AppliedDate = new DateTime(2024, 03, 04),
                TotalRecords = 0,
                User = null
            };

            _mockapplicaionRepository.Setup(u => u.GetDetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((UserVacancyRequest)null);

            _mockapplicaionRepository.Setup(u => u.CreateAsync(It.IsAny<UserVacancyRequest>()))
                .ReturnsAsync(_applicationlist[0]);

            // Act
            var result = await _applicationController.CreateApplication(request);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Applied Successfully"));

            var responseResult = response.Result as UserVacancyResponseDto;
            Assert.That(responseResult.ApplicationStatus, Is.EqualTo("SUBMITTED"));
        }

        [Test]
        public async Task ProcessApplication_ApplicationDoesNotExists_ReturnsFailure()
        {
            // Arrange
            var request = new statusChangeRequestDto()
            {
                status = null,
                id = Guid.NewGuid(),
            };

            _mockapplicaionRepository.Setup(u => u.GetDetailByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((UserVacancyRequest)null);

            // Act
            var result = await _applicationController.ProcessApplication(request);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Job Application Not Found"));
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("randomstatus")]
        public async Task ProcessApplication_InvalidStatus_ReturnsFailure(string appStatus)
        {
            // Arrange
            var request = new statusChangeRequestDto()
            {
                status = appStatus,
                id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
            };

            var exisitngApplication = new UserVacancyRequest()
            {
                Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                Vacancy = new Vacancy()
                {
                    Id = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy = "Green Solutions",
                    PublishedDate = new DateTime(2024, 02, 29),
                    NoOfVacancies = 2,
                    MinimumQualification = "BE in Civil",
                    JobTitle = "Site Manager",
                    JobDescription = "random description for site manager job",
                    ExperienceRequired = "3 to 5 years",
                    LastDate = new DateTime(2024, 03, 08),
                    MinimumSalary = 50000,
                    MaximumSalary = 30000
                },
                UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                AppliedDate = new DateTime(2024, 03, 11),
                ApplicationStatus = "SUBMITTED",
                TotalRecords = 0,
            };

            _mockapplicaionRepository.Setup(u => u.GetDetailByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(exisitngApplication);

            // Act
            var result = await _applicationController.ProcessApplication(request);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Application Status should be either Accepted or Rejected."));

            _mockapplicaionRepository.Verify(u => u.GetDetailByIdAsync(request.id), Times.Once);
        }

        [TestCase(SD.Status_Accepted)]
        [TestCase(SD.Status_Rejected)]
        public async Task ProcessApplication_ApplicationExistsAndValidStatus_ReturnsSuccess(string appStatus)
        {
            // Arrange
            var request = new statusChangeRequestDto()
            {
                status = appStatus,
                id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
            };

            var exisitngApplication = new UserVacancyRequest()
            {
                Id = new Guid("1A3D8C7E-DB11-45FE-C2B6-08DC3C361C6E"),
                VacancyId = new Guid("61EDDA4F-EBDD-42F9-ED13-08DC39205BD7"),
                Vacancy = new Vacancy()
                {
                    Id = new Guid("61edda4f-ebdd-42f9-ed13-08dc39205bd7"),
                    PublishedBy = "Green Solutions",
                    PublishedDate = new DateTime(2024, 02, 29),
                    NoOfVacancies = 2,
                    MinimumQualification = "BE in Civil",
                    JobTitle = "Site Manager",
                    JobDescription = "random description for site manager job",
                    ExperienceRequired = "3 to 5 years",
                    LastDate = new DateTime(2024, 03, 08),
                    MinimumSalary = 50000,
                    MaximumSalary = 30000
                },
                UserId = new Guid("dfb764c1-0dea-45a0-b04f-e138c0e08c22"),
                AppliedDate = new DateTime(2024, 03, 11),
                ApplicationStatus = "SUBMITTED",
                TotalRecords = 0,
            };

            _mockapplicaionRepository.Setup(u => u.GetDetailByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(exisitngApplication);

            exisitngApplication.ApplicationStatus = request.status;
            _mockapplicaionRepository.Setup(u => u.UpdateAsync(It.IsAny<UserVacancyRequest>()))
                .ReturnsAsync(exisitngApplication);

            // Act
            var result = await _applicationController.ProcessApplication(request);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Status Updated Successfully"));

            _mockapplicaionRepository.Verify(u => u.GetDetailByIdAsync(request.id), Times.Once);
            _mockapplicaionRepository.Verify(u => u.UpdateAsync(exisitngApplication), Times.Once);

            var responseResult = response.Result as UserVacancyResponseDto;
            Assert.That(responseResult.ApplicationStatus, Is.EqualTo(appStatus));
        }

        [Test]
        public async Task Pagination_InvalidRequest_ReturnsFailure()
        {
            // Arrange
            List<SP_VacancyRequestDto> requestArray = [_request1, _request2, _request3, _request4];
            List<IActionResult> resultArray = [];

            // Act
            foreach (var request in requestArray)
            {
                var result = await _applicationController.Pagination(request);
                resultArray.Add(result);
            }

            // Assert
            foreach (var result in resultArray)
            {
                var okResult = result as ObjectResult;
                Assert.That(okResult, Is.Not.Null);

                var response = okResult.Value as ResponseDto;
                Assert.That(response, Is.Not.Null);
                Assert.That(response.IsSuccess, Is.False);
                Assert.That(response.Message, Is.EqualTo("One or more of the Following Attributes are Invalid (VacancyId, StartIndex, PageSize)"));
            }
        }

        [Test]
        public async Task Pagination_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            _mockapplicaionRepository.Setup(u => u.GetAllVacancyByPageAsync(It.IsAny<SP_VacancyRequestDto>()))
                .Returns(_applicationlist2);

            _mockprofileRepository.Setup(u => u.GetUsers(It.IsAny<List<Guid>>()))
                .ReturnsAsync(_userlist);

            // Act
            var result = await _applicationController.Pagination(_request);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.Empty);

            _mockapplicaionRepository.Verify(u => u.GetAllVacancyByPageAsync(_request), Times.Once);
        }
    }
}
