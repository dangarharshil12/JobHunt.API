using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHunt.Services.JobSeekerAPI.Models
{
    public class UserExperience
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User? User { get; set; }
        public string CompanyName { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string CompanyUrl { get; set; }
        public string Designation { get; set; }
        public string JobDescription { get; set; }
    }
}
