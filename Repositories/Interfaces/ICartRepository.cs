using MiniEcom.Dtos;
using MiniEcom.Models;

namespace MiniEcom.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int userId, int productId);
        Task<IEnumerable<CartItemDto>> GetCartItemsByUserIdAsync(int userId);
    }
}
