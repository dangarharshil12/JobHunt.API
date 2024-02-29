namespace JobHunt.Services.JobSeekerAPI.Models.Dto
{
    public class QualificationResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string QualificationName { get; set; }
        public string University { get; set; }
        public double YearsOfCompletion { get; set; }
        public string GradeOrScore { get; set; }
    }
}
