using JobHunt.Services.EmployerAPI.Models;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IVacancyRepository
    {
        Task<Vacancy> CreateAsync(Vacancy vacancy);
    }
}
