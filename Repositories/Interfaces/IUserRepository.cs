
using MiniEcom.Dtos;
using MiniEcom.Models;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<LoginResultDto> Login(LoginRequestDto user);

        Task<UserProfileDto?> GetUserProfileAsync(int userId);
        Task UpdateUserProfileAsync(int userId, UserProfileUpdateDto dto);

    }
}
