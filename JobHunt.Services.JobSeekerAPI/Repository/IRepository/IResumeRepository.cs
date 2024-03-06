using JobHunt.Services.JobSeekerAPI.Models.Dto;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IResumeRepository
    {
        Task<ResumeDto> Upload(IFormFile file, ResumeDto resume);
    }
}
