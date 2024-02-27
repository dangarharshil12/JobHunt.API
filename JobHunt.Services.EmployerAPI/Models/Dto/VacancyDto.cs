
using System.ComponentModel.DataAnnotations;

namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class VacancyDto
    {
        public string PublishedBy { get; set; }
        public DateOnly PublishedDate { get; set; }
        public int NoOfVacancies { get; set; }
        public string MinimumQualification { get; set; }
        public string JobDescription { get; set; }
        public string ExperienceRequired { get; set; }
        public DateOnly LastDate { get; set; }
        public double MinimumSalary { get; set; }
        public double MaximumSalary { get; set; }
    }
}
