using JobHunt.Services.EmployerAPI.Data;
using JobHunt.Services.EmployerAPI.Models.Dto;
using JobHunt.Services.EmployerAPI.Repository.IRepository;

namespace JobHunt.Services.EmployerAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;

        public ImageRepository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public async Task<CompanyLogoDTO> Upload(IFormFile file, CompanyLogoDTO companyLogoDTO)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{companyLogoDTO.FileName}{companyLogoDTO.FileExtension}");
            FileInfo oldFile = new FileInfo(localPath);
            if (oldFile.Exists)
            {
                oldFile.Delete();
            }

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{companyLogoDTO.FileName}{companyLogoDTO.FileExtension}";

            companyLogoDTO.Url = urlPath;
            return companyLogoDTO;
        }
    }
}
