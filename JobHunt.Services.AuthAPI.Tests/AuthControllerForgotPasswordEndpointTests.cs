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


        [SetUp]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _mockTokenRepository = new Mock<ITokenRepository>();
            _controller = new AuthController(_mockUserManager.Object, _mockTokenRepository.Object);
        }

        [Test]
        public async Task ForgotPassword_WithValidEmail_ShouldSuccess()
        {
            // Arrange
            var request = new LoginRequstDto { Email = "test@example.com", Password = "Test@123" };
            var user = new ApplicationUser();

            _mockUserManager.Setup(m => m.FindByEmailAsync(request.Email))
                            .ReturnsAsync(user);

            _mockUserManager.Setup(m => m.GeneratePasswordResetTokenAsync(user))
                            .ReturnsAsync("randomtoken");

            _mockUserManager.Setup(m => m.ResetPasswordAsync(user, "randomtoken", request.Password))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Password Reset Successfull"));
        }

        [Test]
        public async Task ForgotPassword_WithInvalidEmail_ShouldFail()
        {
            // Arrange
            var request = new LoginRequstDto { Email = "nonexistent@example.com", Password = "Test@123" };

            _mockUserManager.Setup(m => m.FindByEmailAsync(request.Email))
                            .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _controller.ForgotPassword(request);

            // Assert
            var badRequestResult = result as OkObjectResult;
            var response = badRequestResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("User does not exist. Please Register"));
        }

    }
}
