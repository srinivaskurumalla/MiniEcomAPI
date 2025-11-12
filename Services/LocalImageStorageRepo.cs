namespace MiniEcom.Services
{
    public class LocalImageStorageRepo : IImageStorageRepo
    {
        private readonly IWebHostEnvironment _env;

        public LocalImageStorageRepo(IWebHostEnvironment env)
        {
            _env = env;
        }
        public Task DeleteImageAsync(string relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string folderPath)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, folderPath);

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderPath, fileName).Replace("\\", "/");
        }
    }
}
