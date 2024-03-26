using JobHunt.Services.JobSeekerAPI.Models.Dto;

namespace JobHunt.Services.JobSeekerAPI.Repository.IRepository
{
    public interface IUploadRepository
    {
        Task<UploadDto> Upload(IFormFile file, UploadDto resume);
        Task<UploadDto> UploadImage(IFormFile file, UploadDto image);
    }
}
