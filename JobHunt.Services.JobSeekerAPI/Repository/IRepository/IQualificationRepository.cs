using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IQualificationRepository
    {
        Task<List<Qualification>?> GetAllAsync();
        Task<Qualification> CreateAsync(Qualification qualification);
    }
}
