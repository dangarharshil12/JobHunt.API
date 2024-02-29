namespace JobHunt.Services.JobSeekerAPI.Models.Dto
{
    public class UserExperienceRequestDto
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public string CompanyUrl { get; set; }
        public string Designation { get; set; }
        public string JobDescription { get; set; }
    }
}
