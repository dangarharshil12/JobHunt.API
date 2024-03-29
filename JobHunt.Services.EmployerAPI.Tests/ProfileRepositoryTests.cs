using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace JobHunt.Services.EmployerAPI.Tests
{
    [TestFixture]
    public class ProfileRepositoryTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private ProfileRepository _profileRepository;

        [SetUp]
        public void Setup()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _profileRepository = new ProfileRepository(_httpClientFactoryMock.Object);
        }

        [Test]
        public async Task GetUsers_WithValidUsers_ShouldReturnUserList()
        {
            // Arrange
            var users = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var expectedUserDtoList = new List<UserDto>
            {
                new UserDto {  },
                new UserDto {  }
            };

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedUserDtoList), Encoding.UTF8, "application/json")
            };

            var httpClientHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpClientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(httpClientHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:7284/api/jobSeeker/getUsers")
            };

            _httpClientFactoryMock.Setup(x => x.CreateClient("Profile")).Returns(httpClient);

            // Act
            var result = await _profileRepository.GetUsers(users);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(expectedUserDtoList.Count));
        }
    }
}
