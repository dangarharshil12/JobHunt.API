using JobHunt.Services.EmployerAPI.Models;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface ICompanyRepository
    {
        Task<Employer> CreateAsync(Employer employer);
        Task<Employer?> UpdateAsync(Employer employer);
    }
}
