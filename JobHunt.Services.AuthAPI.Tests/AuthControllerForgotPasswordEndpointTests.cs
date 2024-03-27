using JobHunt.Services.AuthAPI.Controllers;
using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Models.Dto;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;


namespace JobHunt.Services.AuthAPI.Tests
{
    [TestFixture]
    public class AuthControllerForgotPasswordEndpointTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private AuthController _controller;
        private Mock<ITokenRepository> _mockTokenRepository;
        private ApplicationUser user;

        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _mockTokenRepository = new Mock<ITokenRepository>();
            _controller = new AuthController(_mockUserManager.Object, _mockTokenRepository.Object);
        }

        public AuthControllerForgotPasswordEndpointTests()
        {
            user = new ApplicationUser
            {
                FullName = "Test Test",
                UserName = "test@example.com",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
            };
        }

        [Test]
        public async Task ForgotPassword_WithValidEmail_ShouldSuccess()
        {
            // Arrange
            var request = new LoginRequstDto { Email = "test@example.com", Password = "Test@123" };
            

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync(user);

            _mockUserManager.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()))
                            .ReturnsAsync("randomtoken");

            _mockUserManager.Setup(m => m.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult.Value as ResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Password Reset Successfull"));

            _mockUserManager.Verify(u => u.FindByEmailAsync(request.Email), Times.Once);
            _mockUserManager.Verify(u => u.GeneratePasswordResetTokenAsync(user), Times.Once);
            _mockUserManager.Verify(u => u.ResetPasswordAsync(user, "randomtoken", request.Password), Times.Once);
        }

        [Test]
        public async Task ForgotPassword_WithInvalidEmail_ShouldFail()
        {
            // Arrange
            var request = new LoginRequstDto { Email = "nonexistent@example.com", Password = "Test@123" };

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var requestResult = result as OkObjectResult;
            var response = requestResult.Value as ResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("User does not exist. Please Register"));

            _mockUserManager.Verify(u => u.FindByEmailAsync(request.Email), Times.Once);
        }

        [TestCase("", "")]
        [TestCase("test@eamil.com", "")]
        [TestCase("", "newpassword")]
        [TestCase(null, null)]
        [TestCase(null, "newpassword")]
        [TestCase("", null)]
        public async Task ForgotPassword_WithEmptyRequest_ShouldFail(string email, string password)
        {
            // Arrange
            var request = new LoginRequstDto { Email = email, Password = password };

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var requestResult = result as OkObjectResult;
            var response = requestResult.Value as ResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Incomplete Credentials (Either Email or Password or both are Empty)"));
            
            _mockUserManager.Verify(u => u.FindByEmailAsync(request.Email), Times.Never);
        }
    }
}
