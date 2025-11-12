using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniEcom.Api.Dtos;
using MiniEcom.Api.Utilities;
using MiniEcom.Dtos;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;
using MiniEcom.Utilities;

namespace MiniEcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public UsersController(IUserRepository repo) { _repo = repo; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var exists = await _repo.GetByEmailAsync(dto.Email);
            if (exists != null) return Conflict(new { message = "Email already in use" });

            var existsUser = await _repo.GetByUsernameAsync(dto.Username);
            if (existsUser != null) return Conflict(new { message = "Username already in use" });

            PasswordHasher.CreatePasswordHash(dto.Password, out var hash, out var salt);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                DisplayName = dto.Username
            };

            var created = await _repo.CreateAsync(user);

            return CreatedAtAction(null, new { id = created.Id }, new { created.Id, created.Username, created.Email });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
           LoginResultDto result = await _repo.Login(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1)
                return Unauthorized("Invalid user token");

            var user = await _repo.GetUserProfileAsync(userId);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UserProfileUpdateDto dto)
        {
            var userId = UserHelper.GetUserId(User);
            if (userId == -1)
                return Unauthorized("Invalid user token");

            await _repo.UpdateUserProfileAsync(userId, dto);
            return Ok(new { message = "Profile updated successfully" });
        }
    }
}
