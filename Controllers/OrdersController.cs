using Microsoft.AspNetCore.Mvc;
using MiniEcom.Api.Dtos;
using MiniEcom.Repositories.Interfaces;

namespace MiniEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        public OrdersController(IOrderRepository repo) { _repo = repo; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ordNo = await _repo.CreateOrderAsync(dto);
            return Ok(new { orderNumber = ordNo });
        }
    }
}
