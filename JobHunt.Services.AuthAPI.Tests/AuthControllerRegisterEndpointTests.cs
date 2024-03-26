using JobHunt.Services.AuthAPI.Controllers;
using JobHunt.Services.AuthAPI.Models.Dto;
using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using JobHunt.Services.AuthAPI.Utility;


namespace JobHunt.Services.AuthAPI.Tests
{
    [TestFixture]
    public class AuthControllerRegisterEndpointTests
    {
        private AuthController _authController;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<ITokenRepository> _mockTokenRepository;
        private RegisterRequestDto user;

        public AuthControllerRegisterEndpointTests()
        {
            user = new RegisterRequestDto
            {
                FirstName = "testfirst",
                LastName = "testlast",
                Password = "Test@123",
                Email = "test123@eamil.com",
                PhoneNumber = "1234567890",
            };
        }

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _mockTokenRepository = new Mock<ITokenRepository>();

            _authController = new AuthController(_mockUserManager.Object, _mockTokenRepository.Object);
        }

        [Test]
        public async Task Register_UserExists_ReturnsBadRequest()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

            // Act
            var result = await _authController.Register(new RegisterRequestDto());

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("User with this email already exist. Please Login"));
        }

        [TestCase(SD.RoleJobSeeker)]
        [TestCase(SD.RoleEmployer)]
        public async Task Register_UserDoesNotExist_ReturnsSuccess(string userRole)
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            user.Role = userRole;
            var result = await _authController.Register(user);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var responseDto = okResult.Value as ResponseDto;
            Assert.That(responseDto, Is.Not.Null);

            Assert.That(responseDto.IsSuccess, Is.True);
            Assert.That("User Registration Successful", Is.EqualTo(responseDto.Message));
        }
    }
}
