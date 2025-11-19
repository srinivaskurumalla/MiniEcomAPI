using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniEcom.Api.Dtos;
using MiniEcom.Controllers;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;
using MiniEcom.Utilities;
using System.Security.Claims;

namespace MiniEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartRepository _repo;
        public CartController(ICartRepository repo) { _repo = repo; }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddToCartDto dto)
        {
            if (CurrentUserId == null)
                return Unauthorized("Please login to add to cart");

            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repo.AddToCartAsync(CurrentUserId.Value, dto.ProductId, dto.Quantity);

            var (uniqueProducts, totalQuantity) = await _repo.GetCartItemCountAsync(CurrentUserId.Value);

            return Ok(new
            {
                message = "Item added to cart successfully",
                uniqueProducts,
                totalQuantity
            });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            if (CurrentUserId == null)
                return Unauthorized("Please login to add to cart");

            await _repo.RemoveFromCartAsync(CurrentUserId.Value, productId);

            var (uniqueProducts, totalQuantity) = await _repo.GetCartItemCountAsync(CurrentUserId.Value);

            return Ok(new
            {
                message = "Removed from cart",
                uniqueProducts,
                totalQuantity
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            if (CurrentUserId == null)
                return Unauthorized("Please login to add to cart");

            var cartItems = await _repo.GetCartItemsByUserIdAsync(CurrentUserId.Value);

            //if (!cartItems.Any())
            //    return NotFound("No cart items found for this user.");
            return Ok(cartItems);

        }
    }
}
