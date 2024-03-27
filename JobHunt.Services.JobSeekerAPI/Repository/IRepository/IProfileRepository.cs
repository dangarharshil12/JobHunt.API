using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IProfileRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserIdAsync(Guid userId);
        Task<List<User>> GetUsersAsync(List<Guid> userList);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(User user);
    }
}
