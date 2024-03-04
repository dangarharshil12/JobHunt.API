namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class UserVacancyRequestDto
    {
        public Guid VacancyId { get; set; }
        public Vacancy? Vacancy { get; set; }
        public Guid UserId { get; set; }
        public UserDto? User { get; set; }
        public DateTime AppliedDate { get; set; }
    }
}
