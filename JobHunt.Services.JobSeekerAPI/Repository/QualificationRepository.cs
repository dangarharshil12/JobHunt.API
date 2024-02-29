using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;

namespace JobHunt.Services.JobSeekerAPI.Repository
{
    public class QualificationRepository : IQualificationRepository
    {
        private readonly ApplicationDbContext _db;

        public QualificationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Qualification> CreateAsync(Qualification qualification)
        {
            await _db.Qualifications.AddAsync(qualification);
            await _db.SaveChangesAsync();
            return qualification;
        }
    }
}
