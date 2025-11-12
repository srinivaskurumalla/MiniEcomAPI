
using MiniEcom.Models;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IImagesRepository
    {
        Task<List<ProductImage>> UploadProductImagesAsync(int productId, List<IFormFile> files, bool isThumbnail = false);
        Task<List<ProductImage>> GetProductImagesAsync(int productId);
        Task DeleteProductImageAsync(int imageId);

    }
}
