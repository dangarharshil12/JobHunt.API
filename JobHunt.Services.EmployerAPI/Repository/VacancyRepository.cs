using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models;
using JobHunt.Services.EmployerAPI.Repository.IRepository;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ApplicationDbContext _db;

        public VacancyRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Vacancy> CreateAsync(Vacancy vacancy)
        {
            await _db.VacancyDetails.AddAsync(vacancy);
            await _db.SaveChangesAsync();
            return vacancy;
        }
    }
}
