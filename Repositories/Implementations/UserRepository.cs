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
        public UserRepository(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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

    }
}
