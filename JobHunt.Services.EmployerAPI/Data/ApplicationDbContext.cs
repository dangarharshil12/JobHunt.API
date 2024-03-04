using JobHunt.Services.EmployerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Services.EmployerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employer> Employers { get; set; } 
        public DbSet<Vacancy> VacancyDetails { get; set; }
        public DbSet<UserVacancyRequest> UserVacancyRequests { get; set; }
    }
}
