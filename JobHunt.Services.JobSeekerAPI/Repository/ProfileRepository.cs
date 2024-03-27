using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.JobSeekerAPI.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _db;
        public ProfileRepository(ApplicationDbContext db) 
        { 
            _db = db;
        }

        public async Task<List<User>> GetUsersAsync(List<Guid> users)
        {
            return await _db.Users.Where(u => users.Contains(u.Id)).ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingProfile = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (existingProfile != null)
            {
                _db.Entry(existingProfile).CurrentValues.SetValues(user);
                await _db.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task<User?> GetByUserIdAsync(Guid userId)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
