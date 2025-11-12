using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MiniEcom.Api.Dtos;
using MiniEcom.Data;
using MiniEcom.Dtos;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MiniEcom.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _db;
        private readonly string _baseUrl;
        public CartRepository(AppDbContext db, IConfiguration config) {
            _db = db;
            _baseUrl = config["FileSettings:BaseUrl"];
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _db.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            var item = await _db.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);
            if (item == null)
            {
                item = new CartItem { CartId = cart.Id, ProductId = productId, Quantity = quantity };
                _db.CartItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
                _db.CartItems.Update(item);
            }
            cart.LastUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null) return;
            var item = await _db.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);
            if (item == null) return;
            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByUserIdAsync(int userId)
        {

            // Step 1: Find the cart for this user
            var cart = await _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.ProductImages.Where(pi => pi.IsPrimary))
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Step 2: If no cart exists, return empty
            if (cart == null)
                return Enumerable.Empty<CartItemDto>();

            // Step 3: Return cart items with related product info
            var cartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                AddedAt = ci.AddedAt,
                Product = new ProductDto
                {
                    Id = ci.Product.Id,
                    SKU = ci.Product.Sku,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price,
                    StockQuantity = ci.Product.StockQuantity,
                    Images = ci.Product.ProductImages.Select(img => new ProductImageDto
                    {
                        Id = img.Id,
                        FileName = img.FileName ?? string.Empty,
                        ImageUrl = $"{_baseUrl}/{img.FilePath}"

                    }).ToList()
                }
            });
            return cartItems;
        }
    }
}
