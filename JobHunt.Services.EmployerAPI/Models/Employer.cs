using System.ComponentModel.DataAnnotations;

namespace JobHunt.Services.EmployerAPI.Models
{
    public class Employer
    {
        public Guid Id { get; set; }
        [Required]
        public string Organization { get; set; }
        [Required]
        public string OrganizationType { get; set; }
        [Required]
        public string CompanyEmail { get; set; }
        [Required]
        public string CompanyPhone { get; set; }
        [Required]
        public int NoOfEmployees { get; set; }
        [Required]
        public int StartYear { get; set; }
        [Required]
        public string About { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public string ImageUrl { get; set; }
    }
}
