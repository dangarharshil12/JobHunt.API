using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Repository.IRepository;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserVacancyRequest> CreateAsync(UserVacancyRequest request)
        {
            await _db.UserVacancyRequests.AddAsync(request);
            await _db.SaveChangesAsync();

            return request;
        }
    }
}
