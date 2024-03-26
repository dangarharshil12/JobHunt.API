using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Models.Dto;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IApplicationRepository
    {
        Task<UserVacancyRequest?> GetDetailByIdAsync(Guid id);
        Task<UserVacancyRequest?> GetDetailAsync(Guid userId, Guid vacancyId);
        Task<List<UserVacancyRequest>> GetAllByUserIdAsync(Guid userId);
        Task<List<UserVacancyRequest>> GetAllByVacancyIdAsync(Guid vacancyId);
        Task<UserVacancyRequest> CreateAsync(UserVacancyRequest request);
        Task<UserVacancyRequest?> UpdateAsync(UserVacancyRequest request);
        List<UserVacancyRequest> GetAllVacancyByPageAsync(SP_VacancyRequestDto request);
    }
}
