using MiniEcom.Models;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(string? q, int page = 1, int pageSize = 20);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);

        Task AddProductImageAsync(ProductImage image);
        Task<IEnumerable<Category>> GetCategoriesAsync();

    }
}
