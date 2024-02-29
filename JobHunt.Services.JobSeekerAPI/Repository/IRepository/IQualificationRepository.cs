using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IQualificationRepository
    {
        Task<List<Qualification>?> GetAllByUserIdAsync(Guid id);
        Task<Qualification?> GetByIdAsync(Guid id);
        Task<Qualification> CreateAsync(Qualification qualification);
        Task<Qualification?> UpdateAsync(Qualification qualification);
        Task<Qualification> DeleteAsync(Qualification qualification);
    }
}
