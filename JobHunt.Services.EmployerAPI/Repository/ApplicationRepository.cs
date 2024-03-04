using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<UserVacancyRequest>> GetAllByUserIdAsync(Guid userId)
        {
            var result = await _db.UserVacancyRequests.Where(request => request.UserId == userId).Include(u => u.Vacancy).ToListAsync();
            return result;
        }

        public async Task<List<UserVacancyRequest>> GetAllByVacancyIdAsync(Guid vacancyId)
        {
            var result = await _db.UserVacancyRequests.Where(request => request.VacancyId == vacancyId).Include(u => u.Vacancy).ToListAsync();
            return result;
        }

        public async Task<UserVacancyRequest?> GetDetailAsync(Guid userId, Guid vacancyId)
        {
            return await _db.UserVacancyRequests.FirstOrDefaultAsync(u => u.VacancyId == vacancyId && u.UserId == userId);
        }
    }
}
