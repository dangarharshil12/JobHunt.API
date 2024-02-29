using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.JobSeekerAPI.Repository
{
    public class QualificationRepository : IQualificationRepository
    {
        private readonly ApplicationDbContext _db;

        public QualificationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Qualification>?> GetAllByUserIdAsync(Guid id)
        {
            return await _db.Qualifications.Where(u => u.UserId == id).ToListAsync();
        }

        public async Task<Qualification?> GetByIdAsync(Guid id)
        {
            Qualification qualification = await _db.Qualifications.FirstOrDefaultAsync(x => x.Id == id);
            if(qualification == null)
            {
                return null;
            }
            return qualification;
        }

        public async Task<Qualification> CreateAsync(Qualification qualification)
        {
            await _db.Qualifications.AddAsync(qualification);
            await _db.SaveChangesAsync();
            return qualification;
        }

        public async Task<Qualification?> UpdateAsync(Qualification qualification)
        {
            var existingQualification = await _db.Qualifications.FirstOrDefaultAsync(x => x.Id == qualification.Id);

            if (existingQualification != null)
            {
                _db.Entry(existingQualification).CurrentValues.SetValues(qualification);
                await _db.SaveChangesAsync();
                return qualification;
            }
            return null;
        }
    }
}
