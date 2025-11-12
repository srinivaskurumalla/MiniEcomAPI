using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Api.Dtos;
using MiniEcom.Repositories.Interfaces;
using System.Text.Json;

namespace MiniEcom.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) { _db = db; }

        public async Task<string> CreateOrderAsync(CreateOrderDto dto)
        {
            // Build JSON for stored procedure
            var items = dto.Items.Select(i => new { i.ProductId, i.Quantity, i.UnitPrice }).ToArray();
            var itemsJson = JsonSerializer.Serialize(items);

            var conn = _db.Database.GetDbConnection();
            await using (conn)
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "dbo.usp_CreateOrderAndDeductStock";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                var pUser = new SqlParameter("@UserId", dto.UserId);
                var pShip = new SqlParameter("@ShippingAddressId", dto.ShippingAddressId);
                var pBill = new SqlParameter("@BillingAddressId", dto.BillingAddressId ?? (object)DBNull.Value);
                var pItems = new SqlParameter("@ItemsJson", System.Data.SqlDbType.NVarChar) { Value = itemsJson };
                var pOut = new SqlParameter("@OrderNumberOut", System.Data.SqlDbType.NVarChar, 50) { Direction = System.Data.ParameterDirection.Output };

                cmd.Parameters.Add(pUser);
                cmd.Parameters.Add(pShip);
                cmd.Parameters.Add(pBill);
                cmd.Parameters.Add(pItems);
                cmd.Parameters.Add(pOut);

                await cmd.ExecuteNonQueryAsync();

                var orderNumber = pOut.Value?.ToString() ?? string.Empty;
                return orderNumber;
            }
        }
    }
}
