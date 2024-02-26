using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Employer?> GetByEmailAsync(string email)
        {
            return await _db.Employers.FirstOrDefaultAsync(u => u.CreatedBy.ToLower() == email.ToLower());
        }

        public async Task<Employer> CreateAsync(Employer employer)
        {
            await _db.Employers.AddAsync(employer);
            await _db.SaveChangesAsync();

            return employer;
        }

        
    }
}
