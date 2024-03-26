using JobHunt.Services.EmployerAPI.Models.Dto;

namespace JobHunt.Services.EmployerAPI.Repository.IRepository
{
    public interface IImageRepository
    {
        Task<CompanyLogoDTO> Upload(IFormFile file, CompanyLogoDTO companyLogoDTO);
    }
}
