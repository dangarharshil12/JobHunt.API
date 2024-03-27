using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Repositories;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using JobHunt.Services.AuthAPI.Utility;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobHunt.Services.AuthAPI.Tests
{
    [TestFixture]
    public class TokenRepositoryTests
    {
        private ITokenRepository _tokenRepository;
        private IConfiguration _configuration;
        private ApplicationUser _user1;
        private ApplicationUser _user2;

        public TokenRepositoryTests()
        {
            _user1 = new ApplicationUser()
            {
                Id = "154914e8-ddcf-4355-8319-27ef76d9c205",
                FullName = "First2 Last2",
                UserName = "demoemployer2@email.com",
                Email = "demoemployer2@email.com",
                PhoneNumber = "9876543210",
            };
            _user2 = new ApplicationUser()
            {
                Id = "2d967414-5688-4dc0-a23a-dbf7880b19d6",
                FullName = "First Last",
                UserName = "demoemployer@email.com",
                Email = "demoemployer@email.com",
                PhoneNumber = "9876543210",
            };
        }

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "Thisisasecretkeyforjobhuntauthapijwttoken"},
                {"Jwt:Issuer", "https://localhost:7195"},
                {"Jwt:Audience", "https://localhost:4200"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenRepository = new TokenRepository(_configuration);
        }

        [TestCase(SD.RoleJobSeeker)]
        [TestCase(SD.RoleEmployer)]
        public void CreateJwtToken_UserandRole_ReturnsJwtToken(string role)
        {
            // Arrange
            List<string> Roles = [role];

            // Act
            var result = _tokenRepository.CreateJwtToken(_user1, Roles);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(result);
            var token = jsonToken as JwtSecurityToken;
            // Arrange
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            Assert.That(token.Claims.First(c => c.Type == "Id").Value, Is.EqualTo(_user1.Id));
            Assert.That(token.Claims.First(c => c.Type == ClaimTypes.Name).Value, Is.EqualTo(_user1.FullName));
            Assert.That(token.Claims.First(c => c.Type == ClaimTypes.Email).Value, Is.EqualTo(_user1.Email));
        }

        [TestCase(SD.RoleJobSeeker)]
        [TestCase(SD.RoleEmployer)]
        public void CreateJwtToken_DifferentUsers_ShouldReturnUniqueJwtTokens(string role)
        {
            // Arrange
            List<string> Roles = [role];

            // Act
            var result1 = _tokenRepository.CreateJwtToken(_user1, Roles);
            var result2 = _tokenRepository.CreateJwtToken(_user2, Roles);

            // Arrange
            Assert.That(result1, Is.Not.Null);
            Assert.That(result1, Is.Not.Empty);
            Assert.That(result2, Is.Not.Null);
            Assert.That(result2, Is.Not.Empty);

            Assert.That(result1, Is.Not.EqualTo(result2));
        }
    }
}
