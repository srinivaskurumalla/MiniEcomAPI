namespace MiniEcom.Services
{
    public interface IImageStorageRepo
    {
        Task<string> SaveImageAsync(IFormFile file, string folderPath);
        Task DeleteImageAsync(string relativePath);
    }
}
