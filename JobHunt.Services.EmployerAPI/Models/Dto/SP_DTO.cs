namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class SP_DTO
    {
        public Guid Id { get; set; }
        public Guid VacancyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public int? TotalRecords { get; set; }

    }
}
