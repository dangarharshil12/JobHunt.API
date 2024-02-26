namespace JobHunt.Services.EmployerAPI.Models.Dto
{
    public class EmployerDto
    {
        public string Organization { get; set; }
        public string OrganizationType { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public int NoOfEmployees { get; set; }
        public int StartYear { get; set; }
        public string About { get; set; }
        public string CreatedBy { get; set; }
    }
}
