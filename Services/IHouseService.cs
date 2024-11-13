using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IHouseService
    {
        Task<HouseDto> GetHouseById(int houseId);
        Task<List<HouseDto>> GetHousesByUserId(int userId);
        Task<int> CreateHouse(HouseDto houseDto);
        Task<bool> DeleteHouseById(int houseId);
        Task<bool> UpdateHouse(int houseId, HouseDto houseDto);
    }
}