using Microsoft.AspNetCore.Mvc;
using MiniEcom.Api.Dtos;
using MiniEcom.Models;
using MiniEcom.Dtos;
using MiniEcom.Repositories.Interfaces;

namespace MiniEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly IImagesRepository _imagesRepo;
        public ProductsController(IProductRepository repo, IImagesRepository imagesRepo)
        {
            _repo = repo;
            _imagesRepo = imagesRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? q, [FromQuery] int page = 1)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var products = await _repo.SearchAsync(q, page);
            var dto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                SKU = p.Sku,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                shortDescription = p?.ShortDescription,
                Images = p.ProductImages.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    FileName = i.FileName,
                    ImageUrl = $"{baseUrl}/{i.FilePath}"
                }).ToList()

            });
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound("Product not found");

            var dto = new ProductDetailsDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                Mrp = product.Mrp,
                StockQuantity = product.StockQuantity,
                Images = product.ProductImages.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    FileName = i.FileName,
                    ImageUrl = $"{baseUrl}/{i.FilePath}"
                }).ToList()
            };
            return Ok(dto!);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Sku = dto.SKU,
                Name = dto.Name,
                ShortDescription = dto.ShortDescription,
                LongDescription = dto.LongDescription,
                Price = dto.Price,
                Mrp = dto.MRP,
                TaxPercent = dto.TaxPercent,
                StockQuantity = dto.StockQuantity,
                IsActive = dto.IsActive,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddProduct(product);

            //  Handle Image Uploads
            if (dto.Images != null && dto.Images.Count > 0)
            {
                await _imagesRepo.UploadProductImagesAsync(product.Id, dto.Images);
            }
            return Ok(new { message = "Product added successfully", id = product.Id });

        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product is null) return NotFound("Product not found");
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;


            await _repo.UpdateProduct(product);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _repo.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = $"Product with id {id} not found" });

            await _repo.DeleteProduct(id);
            return Ok(new { message = "Product deleted successfully" });
        }


        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _repo.GetCategoriesAsync();
           var cats = categories.Select(c =>  new { c.Id, c.Name });
            return Ok(cats);
        }



    }
}
