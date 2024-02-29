using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.JobSeekerAPI.Repository
{
    public class ExperienceRepository : IExperienceRepository
    {
        private readonly ApplicationDbContext _db;

        public ExperienceRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserExperience> CreateAsync(UserExperience userExperience)
        {
            await _db.UserExperiences.AddAsync(userExperience);
            await _db.SaveChangesAsync();
            return userExperience;
        }

        public Task<UserExperience?> DeleteAsync(UserExperience userExperience)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserExperience>?> GetAllByUserIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserExperience?> GetByIdAsync(Guid id)
        {
            var result = await _db.UserExperiences.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public Task<UserExperience?> UpdateAsync(UserExperience userExperience)
        {
            throw new NotImplementedException();
        }
    }
}
