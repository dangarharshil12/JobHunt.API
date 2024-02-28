using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;

namespace JobHunt.Services.JobSeekerAPI.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _db;
        public ProfileRepository(ApplicationDbContext db) 
        { 
            _db = db;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
