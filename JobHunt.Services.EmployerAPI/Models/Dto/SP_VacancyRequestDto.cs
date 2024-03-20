namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class SP_VacancyRequestDto
    {
        //public Guid VacancyId { get; set; }
        //public int PageNumber { get; set; }
        //public int PageSize { get; set; }

        public string? SearchText { get; set; }
        public string? SortCoumnName { get; set; }
        public string? SortCoumnDirection { get; set; }
        public int StartIndex { get; set; }
        public int PageSize { get; set; }
        public Guid VacancyId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? ApplicationStatus { get; set; }
    }
}
