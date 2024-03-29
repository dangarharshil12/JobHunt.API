using JobHunt.Services.AuthAPI.Controllers;
using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Models.Dto;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using JobHunt.Services.AuthAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace JobHunt.Services.AuthAPI.Tests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _authController;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<ITokenRepository> _mockTokenRepository;
        private RegisterRequestDto user;
        private LoginRequstDto _loginRequest;
        private LoginResponseDto _loginResponse;
        private ApplicationUser appUser;
        private ApplicationUser _user;

        public AuthControllerTests()
        {
            user = new RegisterRequestDto
            {
                FirstName = "testfirst",
                LastName = "testlast",
                Password = "Test@123",
                Email = "test123@eamil.com",
                PhoneNumber = "1234567890",
            };

            _loginRequest = new LoginRequstDto
            {
                Email = "test@email.com",
                Password = "password"
            };

            _loginResponse = new LoginResponseDto
            {
                UserId = "154914e8-ddcf-4355-8319-27ef76d9c205",
                FullName = "Test Test",
                Email = "test@email.com",
                Token = "jwtgeneratedtoken"
            };

            _user = new ApplicationUser
            {
                FullName = "Test Test",
                UserName = "test@example.com",
                Email = "test@example.com",
                PhoneNumber = "1234567890",
            };

            appUser = new ApplicationUser
            {
                FullName = user.FirstName + " " + user.LastName,
                UserName = user.Email?.Trim(),
                Email = user.Email?.Trim(),
                PhoneNumber = user.PhoneNumber?.Trim(),
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
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(_user);

            // Act
            var result = await _authController.Register(user);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("User with this email already exist. Please Login"));

            _mockUserManager.Verify(x => x.FindByEmailAsync(user.Email), Times.Once);
            _mockUserManager.Verify(x => x.CreateAsync(_user, user.Password), Times.Never);
            _mockUserManager.Verify(x => x.AddToRoleAsync(_user, user.Role), Times.Never);
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

            _mockUserManager.Verify(x => x.FindByEmailAsync(user.Email), Times.Once);
        }

        [Test]
        public async Task Login_UserDoesNotExists_ShouldFail()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _authController.Login(_loginRequest);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("User Does Not Exists. Please Register"));

            _mockUserManager.Verify(x => x.FindByEmailAsync(_loginRequest.Email), Times.Once);
        }

        [Test]
        public async Task Login_InvalidPassword_ShouldFail()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(_user);

            _mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _authController.Login(_loginRequest);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.False);
            Assert.That(response.Message, Is.EqualTo("Invalid Login Credentials"));

            _mockUserManager.Verify(x => x.FindByEmailAsync(_loginRequest.Email), Times.Once);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(_user, _loginRequest.Password), Times.Once);
        }

        [TestCase(SD.RoleEmployer)]
        [TestCase(SD.RoleJobSeeker)]
        public async Task Login_UserExists_VaidCredential_ShouldSuccess(string role)
        {
            // Arrange
            _loginResponse.Roles = [role];
            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(_user);

            _mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _mockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync([role]);

            _mockTokenRepository.Setup(m => m.CreateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<List<string>>()))
                .Returns("jwtgeneratedtoken");

            // Act
            var result = await _authController.Login(_loginRequest);

            // Assert
            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var response = okResult.Value as ResponseDto;
            Assert.That(response, Is.Not.Null);

            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Login Successful"));

            var loginResponse = response.Result as LoginResponseDto;
            Assert.That(loginResponse, Is.Not.Null);
            Assert.That(loginResponse.Email, Is.EqualTo(_loginRequest.Email));
            Assert.That(loginResponse.Token, Is.EqualTo(_loginResponse.Token));
            Assert.That(loginResponse.Roles, Is.EquivalentTo(_loginResponse.Roles));

            _mockUserManager.Verify(x => x.FindByEmailAsync(_loginRequest.Email), Times.Once);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(_user, _loginRequest.Password), Times.Once);
            _mockUserManager.Verify(x => x.GetRolesAsync(_user), Times.Once);
            _mockTokenRepository.Verify(x => x.CreateJwtToken(_user, loginResponse.Roles), Times.Once);
        }

        [Test]
        public async Task ForgotPassword_WithValidEmail_ShouldSuccess()
        {
            // Arrange
            var request = new LoginRequstDto { Email = "test@example.com", Password = "Test@123" };

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync(_user);

            _mockUserManager.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()))
                            .ReturnsAsync("randomtoken");

            _mockUserManager.Setup(m => m.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authController.ForgotPassword(request);

            // Assert
            var okResult = result as OkObjectResult;
            var response = okResult.Value as ResponseDto;

            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.True);
            Assert.That(response.Message, Is.EqualTo("Password Reset Successfull"));

            _mockUserManager.Verify(u => u.FindByEmailAsync(request.Email), Times.Once);
            _mockUserManager.Verify(u => u.GeneratePasswordResetTokenAsync(_user), Times.Once);
            _mockUserManager.Verify(u => u.ResetPasswordAsync(_user, "randomtoken", request.Password), Times.Once);
        }

        [Test]
        public async Task ForgotPassword_WithInvalidEmail_ShouldFail()
        {
            // Arrange
            var request = new LoginRequstDto { Email = "nonexistent@example.com", Password = "Test@123" };

            _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _authController.ForgotPassword(request);

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
            var result = await _authController.ForgotPassword(request);

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
