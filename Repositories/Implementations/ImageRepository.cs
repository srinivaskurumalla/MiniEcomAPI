using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;
using MiniEcom.Services;

namespace MiniEcom.Repositories.Implementations
{
    public class ImageRepository : IImagesRepository
    {
        private readonly IProductRepository _productRepo;
        private readonly IImageStorageRepo _imageStorageRepo;
        private readonly AppDbContext _db;

        public ImageRepository(IProductRepository productRepo, IImageStorageRepo imageStorageRepo, AppDbContext db)
        {
            _productRepo = productRepo;
            _imageStorageRepo = imageStorageRepo;
            _db = db;
        }

        public async Task<List<ProductImage>> UploadProductImagesAsync(int productId, List<IFormFile> files, bool isThumbnail = false)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");

            if (product.ProductImages != null && product.ProductImages.Count >= 4 && !isThumbnail)
                throw new Exception("Maximum 4 images allowed");

            // If uploading a thumbnail, reset existing thumbnails
            //if (isThumbnail && product.ProductImages != null)
            //{
            //    foreach (var img in product.ProductImages)
            //        img.IsPrimary = false;
            //}

            var uploadedImages = new List<ProductImage>();
            bool markPrimary = product.ProductImages == null || product.ProductImages.Count == 0;

            foreach (var file in files)
            {
                var relativePath = await _imageStorageRepo.SaveImageAsync(file, $"uploads/products/{productId}");
                var image = new ProductImage
                {
                    FileName = file.FileName,
                    ProductId = productId,
                    FilePath = relativePath,
                    IsPrimary = markPrimary
                };
                markPrimary = false; // Only first one is primary
                await _productRepo.AddProductImageAsync(image);
                uploadedImages.Add(image);
            }

            return uploadedImages;
        }

        public async Task<List<ProductImage>> GetProductImagesAsync(int productId)
        {
           return await _db.ProductImages.Where(pi => pi.ProductId == productId).ToListAsync();
        }

        public async Task DeleteProductImageAsync(Guid imageId) //will implement later
        {
            //var images = await _productRepo.get // you can extend repository to fetch by Id
            //var image = images.FirstOrDefault(i => i.Id == imageId);
            //if (image == null) throw new Exception("Image not found");

            //await _imageStorage.DeleteImageAsync(image.ImageUrl);
            //_repo.DeleteProductImage(image); // Add Delete method in repository
            //await _repo.SaveChangesAsync();
        }

        public Task DeleteProductImageAsync(int imageId)
        {
            throw new NotImplementedException();
        }
    }
}
