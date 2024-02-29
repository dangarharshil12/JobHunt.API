using JobHunt.Services.JobSeekerAPI.Models;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IQualification
    {
        Task<Qualification> CreateAsync(Qualification qualification);
    }
}
