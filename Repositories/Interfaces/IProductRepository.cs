using MiniEcom.Dtos;
using MiniEcom.Models;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllProducts(int page = 1, int pageSize = 5);
        Task<IEnumerable<Product>> GetProductsByTagAsync(string tag);
        Task<SearchResultDto> SearchAsync(string? q);
        Task<Product> AddProduct(Product product);
        Task AddProductTags(int productId, List<string> tags);
        Task<Product?> UpdateProduct(int id, ProductUpdateDto dto);
        Task<bool> DeleteProduct(int id);

        Task AddProductImageAsync(ProductImage image);
        Task<IEnumerable<Category>> GetCategoriesAsync();

    }
}
