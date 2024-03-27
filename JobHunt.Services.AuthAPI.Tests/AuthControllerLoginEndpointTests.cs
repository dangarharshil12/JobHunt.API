using Azure.Core;
using JobHunt.Services.AuthAPI.Controllers;
using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Models.Dto;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using JobHunt.Services.AuthAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobHunt.Services.AuthAPI.Tests
{
    [TestFixture]
    public class AuthControllerLoginEndpointTests
    {
        private AuthController _authController;
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<ITokenRepository> _mockTokenRepository;
        private LoginRequstDto _loginRequest;
        private LoginResponseDto _loginResponse;
        private ApplicationUser _user;

        public AuthControllerLoginEndpointTests()
        {
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
    }
}
