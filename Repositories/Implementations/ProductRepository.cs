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
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            //Instead of delete, make it inactive
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

          //  _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<Product>> GetAllProducts(int page = 1, int pageSize = 5)
        {
            var query = _db.Products
                  .Include(p => p.ProductImages.Where(i => i.IsPrimary == true)) //Include primary image
                  .Include(p => p.ProductTags);
                 // .Where(p => p.IsActive);


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

        public async Task<IEnumerable<Product>> GetProductsByTagAsync(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return Enumerable.Empty<Product>();

            tag = tag.Trim().ToLower();

            var products = await _db.Products
                  .Include(p => p.ProductImages.Where(i => i.IsPrimary == true))
                  .Include(p => p.ProductTags)
                  .Where(p => p.ProductTags.Any(t => t.Tag.ToLower().Contains(tag)))
                  .ToListAsync();

            return products;
                
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




        public async Task<Product?> UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            // Update main product fields
            product.Name = dto.Name;
            product.ShortDescription = dto.ShortDescription;
            product.LongDescription = dto.LongDescription;
            product.Price = dto.Price;
            product.Mrp = dto.Mrp;
            product.TaxPercent = dto.TaxPercent;
            product.StockQuantity = dto.StockQuantity;
            product.IsActive = dto.IsActive;
            product.CategoryId = dto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();



            //  Replace tags if new ones are provided
            if (dto.Tags != null && dto.Tags.Any())
            {
                var existingTags = _db.ProductTags.Where(t => t.ProductId == product.Id);
                _db.ProductTags.RemoveRange(existingTags);

                var tagEntities = dto.Tags.Select(t => new ProductTag
                {
                    ProductId = product.Id,
                    Tag = t.Trim().ToLower()
                }).ToList();

                await _db.ProductTags.AddRangeAsync(tagEntities);
                await _db.SaveChangesAsync();
            }

            //// ✅ Handle image uploads (if any new ones provided)
            //if (dto.Images != null && dto.Images.Count > 0)
            //{
            //    // Optional: remove old images first if you want a full replace
            //    var oldImages = _db.ProductImages.Where(i => i.ProductId == dto.Id);
            //    _db.ProductImages.RemoveRange(oldImages);
            //    await _db.SaveChangesAsync();

            //    await _imagesRepo.UploadProductImagesAsync(dto.Id, dto.Images);
            //}
            return product;
        }
    }
}
