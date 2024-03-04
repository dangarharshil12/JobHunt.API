using JobHunt.Services.EmployerAPI.Models;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IApplicationRepository
    {
        Task<List<UserVacancyRequest>> GetAllByUserIdAsync(Guid userId);
        Task<List<UserVacancyRequest>> GetAllByVacancyIdAsync(Guid vacancyId);
        Task<UserVacancyRequest> CreateAsync(UserVacancyRequest request);
    }
}
