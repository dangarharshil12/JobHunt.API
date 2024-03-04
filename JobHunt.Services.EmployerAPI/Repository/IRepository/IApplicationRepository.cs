using JobHunt.Services.EmployerAPI.Models;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IApplicationRepository
    {
        Task<UserVacancyRequest> CreateAsync(UserVacancyRequest request);
    }
}
