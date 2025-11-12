using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniEcom.Api.Utilities;
using MiniEcom.Data;
using MiniEcom.Dtos;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MiniEcom.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public UserRepository(AppDbContext db, IConfiguration configuration, IWebHostEnvironment env)
        {
            _db = db;
            _configuration = configuration;
            _env = env;
        }

        public async Task<User?> GetByIdAsync(int id) => await _db.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _db.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User> CreateAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = 2,
                AssignedAt = DateTime.UtcNow
            };
            _db.UserRoles.Add(userRole);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<LoginResultDto> Login(LoginRequestDto user)
        {
            var userExist = await GetByEmailAsync(user.Email);
            if (userExist != null)
            {
               // PasswordHasher.CreatePasswordHash(user.Password, out var hash, out var salt);
                bool verified = PasswordHasher.Verify(user.Password, userExist.PasswordHash, userExist.PasswordSalt);
                if (verified)
                {
                    string token = await GenerateToken(userExist);
                    return new LoginResultDto { Success = true, UserName = userExist.Username, Message = "Login Successful", Token = token };

                }

            }
            return new LoginResultDto { Success = false, UserName="", Message = "Invalid Credentials", Errors = ["Invalid credentials"] };


        }

        private async Task<string> GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["key"]));

            var claims = new List<Claim>
            {
                new("Email",user.Email ?? ""),
                new ("UserName",user.Username ?? ""),
                new ("UserId",user.Id.ToString() ?? ""),
            };

            var roles = user.UserRoles;
            // claims.AddRange(roles.Select(r => new Claim("role", r.Role)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds

                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public Task<UserProfileDto?> GetUserProfileAsync(int userId)
        {
            return _db.Users.Where(u => u.Id == userId)
                .Select(u => new UserProfileDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    DisplayName = u.DisplayName,
                    Phone = u.Phone,
                    ProfileImageUrl = u.ProfileImageUrl,
                    IsEmailConfirmed = u.IsEmailConfirmed,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task  UpdateUserProfileAsync(int userId, UserProfileUpdateDto dto)
        {

            var user = await _db.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.DisplayName = dto.DisplayName ?? user.DisplayName;
            user.Phone = dto.Phone ?? user.Phone;
            user.Email = dto.Email ?? user.Email;
            user.UpdatedAt = DateTime.UtcNow;

            //  Handle profile image upload
            if (dto.ProfileImage != null)
            {
                var uploadDir = Path.Combine(_env.WebRootPath, "uploads", "profiles");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ProfileImage.FileName)}";
                var filePath = Path.Combine(uploadDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.ProfileImage.CopyToAsync(stream);

                user.ProfileImageUrl = $"uploads/profiles/{fileName}";
            }

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
