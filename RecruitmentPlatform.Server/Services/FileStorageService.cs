namespace RecruitmentPlatform.Server.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(IWebHostEnvironment env, ILogger<FileStorageService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> UploadResumeAsync(IFormFile file, string email)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var allowedTypes = new[] { "application/pdf" };
            if (!allowedTypes.Contains(file.ContentType))
                throw new ArgumentException("Only PDF files are allowed");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "resumes");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{email}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"Resume uploaded: {filePath}");
            return $"/uploads/resumes/{fileName}";
        }
    }
}
