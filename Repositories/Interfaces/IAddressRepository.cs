using MiniEcom.Dtos;

namespace MiniEcom.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<AddressDto>> GetUserAddressesAsync(int userId);
        Task<AddressDto?> GetAddressByIdAsync(int id, int userId);
        Task<AddressDto> AddAddressAsync(int userId, AddressCreateDto dto);
        Task<AddressDto?> UpdateAddressAsync(int id, int userId, AddressCreateDto dto);
        Task<bool> DeleteAddressAsync(int id, int userId);
        Task<bool> SetDefaultAddressAsync(int id, int userId);
    }

}
