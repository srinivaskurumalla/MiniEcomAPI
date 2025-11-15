using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Api.Dtos;
using MiniEcom.Repositories.Interfaces;
using System.Text.Json;
using MiniEcom.Dtos;

namespace MiniEcom.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) { _db = db; }

        public async Task<(int OrderId, string OrderNumber, decimal TotalAmount)> CreateOrderAsync(int userId, CheckoutDto dto)
        {
            var parameters = new[]
           {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@ShippingAddressId", dto.ShippingAddressId),
                new SqlParameter("@BillingAddressId", dto.BillingAddressId ?? (object)DBNull.Value),
                new SqlParameter("@PaymentMethod", dto.PaymentMethod),
                new SqlParameter("@Notes", dto.Notes ?? (object)DBNull.Value)
            };

            // Call stored procedure
            var result = await _db.Database
                .SqlQueryRaw<OrderResultDto>(
                    "EXEC usp_CreateOrderAndDeductStock @UserId, @ShippingAddressId, @BillingAddressId, @PaymentMethod, @Notes",
                    parameters)
                .ToListAsync();

            var order = result.FirstOrDefault();
            if (order == null)
                throw new Exception("Order creation failed.");

            return (order.OrderId, order.OrderNumber, order.TotalAmount);
        }
    }
}
