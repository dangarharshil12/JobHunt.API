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

        public async Task<List<UserExperience>?> GetAllByUserIdAsync(Guid id)
        {
            return await _db.UserExperiences.Where(u => u.UserId == id).ToListAsync();
        }

        public async Task<UserExperience?> GetByIdAsync(Guid id)
        {
            var result = await _db.UserExperiences.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<UserExperience> CreateAsync(UserExperience userExperience)
        {
            await _db.UserExperiences.AddAsync(userExperience);
            await _db.SaveChangesAsync();
            return userExperience;
        }

        public async Task<UserExperience?> UpdateAsync(UserExperience userExperience)
        {
            var existingExperience = await _db.UserExperiences.FirstOrDefaultAsync(x => x.Id == userExperience.Id);

            if (existingExperience != null)
            {
                _db.Entry(existingExperience).CurrentValues.SetValues(userExperience);
                await _db.SaveChangesAsync();
                return userExperience;
            }
            return null;
        }

        public async Task<UserExperience> DeleteAsync(UserExperience userExperience)
        {
            _db.UserExperiences.Remove(userExperience);
            await _db.SaveChangesAsync();
            return userExperience;
        }
    }
}
