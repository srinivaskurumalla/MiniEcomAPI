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
        private readonly IConfiguration _config;
        public OrderRepository(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

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

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersByUserIdAsync(int userId)
        {
            var baseUrl = _config["FileSettings:BaseUrl"];
            return await _db.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderPlacedAt)
                .Select(o => new OrderSummaryDto
                {
                    OrderId = o.Id,
                    OrderNumber = o.OrderNumber,
                    TotalAmount = o.TotalAmount,
                    OrderPlacedAt = o.OrderPlacedAt,
                    OrderStatus = o.OrderStatus,
                    IsPaid = o.IsPaid,
                    Items = o.OrderItems.Select(i => new OrderSummaryItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Price = i.UnitPrice,
                        Quantity = i.Quantity,
                        ImageUrl = i.Product.ProductImages
                        .Where(pi => pi.IsPrimary)
                        .Select(pi => baseUrl + "/" + pi.FilePath)
                        .FirstOrDefault() ?? "assets/no-image.png"
                    }).ToList()
                })
                .ToListAsync();
        }


        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId, int userId)
        {
            var order = await _db.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.BillingAddress)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return null;

            var baseUrl = _config["FileSettings:BaseUrl"];

            return new OrderDetailsDto
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                OrderPlacedAt = order.OrderPlacedAt,
                SubTotal = order.SubTotal,
                TaxAmount = order.TaxAmount,
                ShippingCharge = order.ShippingCharge,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                IsPaid = order.IsPaid,
                PaymentMethod = order.PaymentMethod,

                ShippingAddress = new AddressDto
                {
                    RecipientName = order.ShippingAddress.RecipientName,
                    Line1 = order.ShippingAddress.Line1,
                    Line2 = order.ShippingAddress.Line2,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Phone = order.ShippingAddress.Phone
                },

                BillingAddress = order.BillingAddress == null ? null : new AddressDto
                {
                    RecipientName = order.BillingAddress.RecipientName,
                    Line1 = order.BillingAddress.Line1,
                    Line2 = order.BillingAddress.Line2,
                    City = order.BillingAddress.City,
                    State = order.BillingAddress.State,
                    PostalCode = order.BillingAddress.PostalCode,
                    Phone = order.BillingAddress.Phone
                },

                Items = order.OrderItems.Select(oi => new OrderItemDetailsDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    SKU = oi.Sku,
                    ImageUrl = oi.Product.ProductImages.FirstOrDefault(i => i.IsPrimary)?.FilePath != null
                        ? $"{baseUrl}/{oi.Product.ProductImages.First(i => i.IsPrimary).FilePath}"
                        : string.Empty,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

    }
}
