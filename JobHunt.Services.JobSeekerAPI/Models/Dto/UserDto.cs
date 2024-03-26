namespace JobHunt.Services.JobSeekerAPI.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double TotalExperience { get; set; }
        public double ExpectedSalary { get; set; }
        public string ResumeUrl { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
