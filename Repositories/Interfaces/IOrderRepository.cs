using MiniEcom.Models;
using MiniEcom.Api.Dtos;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<string> CreateOrderAsync(CreateOrderDto dto);
    }
}
