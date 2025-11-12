using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniEcom.Dtos;
using MiniEcom.Repositories.Interfaces;
using MiniEcom.Utilities;

namespace MiniEcom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _repo;

        public AddressController(IAddressRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1) return Unauthorized("Please log in");

            var addresses = await _repo.GetUserAddressesAsync(userId);
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1) return Unauthorized();

            var address = await _repo.GetAddressByIdAsync(id, userId);
            if (address == null) return NotFound();

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddressCreateDto dto)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1) return Unauthorized();

            var added = await _repo.AddAddressAsync(userId, dto);
            return Ok(added);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddressCreateDto dto)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1) return Unauthorized();

            var updated = await _repo.UpdateAddressAsync(id, userId, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1) return Unauthorized();

            var deleted = await _repo.DeleteAddressAsync(id, userId);
            return deleted ? Ok() : NotFound();
        }

        [HttpPut("set-default/{id}")]
        public async Task<IActionResult> SetDefault(int id)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1) return Unauthorized();

            var success = await _repo.SetDefaultAddressAsync(id, userId);
            return success ? Ok(new { message = "Default address set" }) : NotFound();
        }
    }

}
