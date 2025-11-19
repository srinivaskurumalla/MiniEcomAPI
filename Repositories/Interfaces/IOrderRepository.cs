using MiniEcom.Api.Dtos;
using MiniEcom.Dtos;
using MiniEcom.Models;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        //Task<string> CreateOrderAsync(CreateOrderDto dto);
        Task<(int OrderId, string OrderNumber, decimal TotalAmount)> CreateOrderAsync(int userId, CheckoutDto dto);
        Task<IEnumerable<OrderSummaryDto>> GetOrdersByUserIdAsync(int userId);

    }
}
