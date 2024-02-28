using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IProfileRepository
    {
        Task<User> CreateAsync(User user);
    }
}
