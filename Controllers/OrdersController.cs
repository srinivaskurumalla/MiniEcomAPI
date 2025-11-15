using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniEcom.Api.Dtos;
using MiniEcom.Dtos;
using MiniEcom.Repositories.Interfaces;
using MiniEcom.Utilities;

namespace MiniEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        public OrdersController(IOrderRepository repo) { _repo = repo; }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1)
                return Unauthorized("Please login to continue.");

            try
            {
                var (orderId, orderNumber, totalAmount) = await _repo.CreateOrderAsync(userId, dto);

                return Ok(new
                {
                    success = true,
                    message = "Order placed successfully!",
                    orderId,
                    orderNumber,
                    totalAmount
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
