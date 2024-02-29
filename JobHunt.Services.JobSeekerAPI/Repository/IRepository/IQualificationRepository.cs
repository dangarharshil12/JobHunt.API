using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IQualificationRepository
    {
        Task<Qualification> CreateAsync(Qualification qualification);
    }
}
