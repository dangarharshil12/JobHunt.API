using JobHunt.Services.JobSeekerAPI.Data;
using JobHunt.Services.JobSeekerAPI.Models.Dto;
using JobHunt.Services.JobSeekerAPI.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace JobHunt.Services.JobSeekerAPI.Repository
{
    public class UploadRespository : IUploadRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        public UploadRespository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public async Task<UploadDto> Upload(IFormFile file, UploadDto resume)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Resumes", $"{resume.FileName}{resume.FileExtension}");
            FileInfo oldFile = new FileInfo(localPath);
            if (oldFile.Exists)
            {
                oldFile.Delete();
            }

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Resumes/{resume.FileName}{resume.FileExtension}";

            resume.Url = urlPath;
            return resume;            
        }

        public async Task<UploadDto> UploadImage(IFormFile file, UploadDto image)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            FileInfo oldFile = new FileInfo(localPath);
            if (oldFile.Exists)
            {
                oldFile.Delete();
            }

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.Url = urlPath;
            return image;
        }
    }
}
