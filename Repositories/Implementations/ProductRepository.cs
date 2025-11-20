using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Dtos;
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

        public async Task<IEnumerable<Product>> GetAllProducts(int page = 1, int pageSize = 5)
        {
            var query = _db.Products
                  .Include(p => p.ProductImages.Where(i => i.IsPrimary == true)) //Include primary image
                  .Where(p => p.IsActive);


            return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
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

        public async Task<SearchResultDto> SearchAsync(string? q)
        {
            var result = new SearchResultDto();

            if (string.IsNullOrWhiteSpace(q))
                return result;

            var searchText = q.Trim().ToLower();

            // Product matches
            result.Products = await _db.Products
                .Include(p => p.ProductImages.Where(i => i.IsPrimary))
                .Where(p => p.IsActive &&
                       (p.Name.Contains(searchText) || p.Sku.Contains(searchText)))
                .Select(p => new ProductSummaryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    ImageUrl = p.ProductImages.FirstOrDefault().FilePath
                })
                .Take(5)
                .ToListAsync();

            // Tag matches
            result.Tags = await _db.ProductTags
                .Where(t => t.Tag.Contains(searchText))
                .Select(t => t.Tag)
                .Distinct()
                .Take(5)
                .ToListAsync();

            return result;
        }




        public async Task<Product> UpdateProduct(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
