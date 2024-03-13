using JobHunt.Services.EmployerAPI.Models;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IVacancyRepository
    {
        Task<List<Vacancy>?> GetAllAsync();
        Task<Vacancy?> GetByIdAsync(Guid id);
        Task<List<Vacancy>?> GetByNameAsync(string name);
        Task<Boolean> CheckApplicationAsync(Guid userId, Guid vacancyId);
        Task<Vacancy> CreateAsync(Vacancy vacancy);
        Task<Vacancy?> UpdateAsync(Vacancy vacancy);
        Task<Vacancy?> DeleteAsync(Vacancy vacancy);
    }
}
