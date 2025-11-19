using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;

namespace MiniEcom.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) { _db = db; }

        public async Task<Product> AddProduct(Product product)
        {

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;

        }

        public async Task AddProductImageAsync(ProductImage image)
        {
            await _db.ProductImages.AddAsync(image);
            await _db.SaveChangesAsync();
        }

        public async Task AddProductTags(int productId, List<string> tags)
        {
            var tagEntities = tags.Select(t => new ProductTag
            {
                ProductId = productId,
                Tag = t.Trim().ToLower()
            }).ToList();

            _db.ProductTags.AddRange(tagEntities);
            await _db.SaveChangesAsync();

        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                {
                    _db.Products.Remove(product);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products
              .Include(p => p.ProductImages)
              .Where(p => p.IsActive && p.Id == id)
              .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string? q, int page = 1, int pageSize = 20)
        {
            var query = _db.Products

                .Include(p => p.ProductImages.Where(i => i.IsPrimary == true)) // <-- always include primary images
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(p =>
                    p.Name.Contains(q) ||
                    p.Sku.Contains(q) ||
                    p.ProductTags.Any(t => t.Tag.Contains(q))
                );

            }

            return await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<Product> UpdateProduct(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
