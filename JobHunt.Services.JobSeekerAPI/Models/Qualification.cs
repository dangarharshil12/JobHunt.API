using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHunt.Services.JobSeekerAPI.Models
{
    public class Qualification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        public string QualificationName { get; set; }
        public string University { get; set; }
        public double YearsOfCompletion { get; set; }
        public string GradeOrScore { get; set; }
    }
}
