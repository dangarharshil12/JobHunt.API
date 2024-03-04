using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Newtonsoft.Json;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var client = _httpClientFactory.CreateClient("Profile");
            var response = await client.GetAsync($"/api/jobSeeker/getUsers");
            var apiContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(apiContent));
        }
    }
}
