using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IExperienceRepository
    {
        Task<List<UserExperience>> GetAllByUserIdAsync(Guid id);
        Task<UserExperience?> GetByIdAsync(Guid id);
        Task<UserExperience> CreateAsync(UserExperience userExperience);
        Task<UserExperience?> UpdateAsync(UserExperience userExperience);
        Task<UserExperience> DeleteAsync(UserExperience userExperience);
    }
}
