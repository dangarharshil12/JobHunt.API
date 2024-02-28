using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ApplicationDbContext _db;

        public VacancyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Vacancy>?> GetAllAsync()
        {
            return await _db.VacancyDetails.ToListAsync();
        }

        public async Task<List<Vacancy>?> GetByNameAsync(string name)
        {
            return await _db.VacancyDetails.Where(u => u.PublishedBy == name).ToListAsync();
        }

        public async Task<Vacancy> CreateAsync(Vacancy vacancy)
        {
            await _db.VacancyDetails.AddAsync(vacancy);
            await _db.SaveChangesAsync();
            return vacancy;
        }
    }
}
