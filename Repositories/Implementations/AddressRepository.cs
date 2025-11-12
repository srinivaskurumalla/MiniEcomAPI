using Microsoft.EntityFrameworkCore;
using MiniEcom.Data;
using MiniEcom.Dtos;
using MiniEcom.Models;
using MiniEcom.Repositories.Interfaces;

namespace MiniEcom.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _db;

        public AddressRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AddressDto> AddAddressAsync(int userId, AddressCreateDto dto)
        {
            if (dto.IsDefault)
            {
                var existDefault = await _db.Addresses.FirstOrDefaultAsync(x => x.UserId == userId && x.IsDefault);
                if (existDefault != null)
                {
                    existDefault.IsDefault = false;
                }
            }

            var address = new Address
            {
                UserId = userId,
                Label = dto.Label,
                RecipientName = dto.RecipientName,
                Line1 = dto.Line1,
                Line2 = dto.Line2,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                Phone = dto.Phone,
                IsDefault = dto.IsDefault,
                CreatedAt = DateTime.UtcNow
            };

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();

            return new AddressDto
            {
                Id = address.Id,
                Label = address.Label,
                RecipientName = address.RecipientName,
                Line1 = address.Line1,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Phone = address.Phone,
                IsDefault = address.IsDefault
            };
        }

        public async Task<bool> DeleteAddressAsync(int id, int userId)
        {
            var address = await _db.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (address == null) return false;

            _db.Addresses.Remove(address);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<AddressDto?> GetAddressByIdAsync(int id, int userId)
        {
            var a = await _db.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (a == null) return null;
            return new AddressDto
            {
                Id = a.Id,
                Label = a.Label,
                RecipientName = a.RecipientName,
                Line1 = a.Line1,
                Line2 = a.Line2,
                City = a.City,
                State = a.State,
                PostalCode = a.PostalCode,
                Phone = a.Phone,
                IsDefault = a.IsDefault
            };
        }

        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId)
        {
            return await _db.Addresses
                   .Where(a => a.UserId == userId)
                   .OrderByDescending(a => a.IsDefault)
                   .Select(a => new AddressDto
                   {
                       Id = a.Id,
                       Label = a.Label,
                       RecipientName = a.RecipientName,
                       Line1 = a.Line1,
                       Line2 = a.Line2,
                       City = a.City,
                       State = a.State,
                       PostalCode = a.PostalCode,
                       Phone = a.Phone,
                       IsDefault = a.IsDefault
                   })
                   .ToListAsync();
        }

        public async Task<bool> SetDefaultAddressAsync(int id, int userId)
        {
            var addresses = await _db.Addresses.Where(a => a.UserId == userId).ToListAsync();
            if (addresses.Count == 0) return false;

            foreach (var addr in addresses)
                addr.IsDefault = addr.Id == id;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<AddressDto?> UpdateAddressAsync(int id, int userId, AddressCreateDto dto)
        {
            var address  = await _db.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if(address == null)  return null;

            if (dto.IsDefault)
            {
                var existDefault = await _db.Addresses.FirstOrDefaultAsync(x => x.UserId == userId && x.IsDefault);
                if (existDefault != null)
                {
                    existDefault.IsDefault = false;
                }
            }

            address.Label = dto.Label;
            address.RecipientName = dto.RecipientName;
            address.Line1 = dto.Line1;
            address.Line2 = dto.Line2;
            address.City = dto.City;
            address.State = dto.State;
            address.PostalCode = dto.PostalCode;
            address.Phone = dto.Phone;
            address.IsDefault = dto.IsDefault;


            await _db.SaveChangesAsync();

            return new AddressDto
            {
                Id = address.Id,
                Label = address.Label,
                RecipientName = address.RecipientName,
                Line1 = address.Line1,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Phone = address.Phone,
                IsDefault = address.IsDefault
            };
        }
    }
}
