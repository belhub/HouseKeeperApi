using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IEqiupmentService
    {
        Task<int> CreateEquipment(EquipmentDto equipmentDto);
        Task<bool> DeleteEquipment(int equipmentId);
        Task<List<EquipmentDto>> GetAllEquipmentByRoomId(int roomId);
        Task<List<EquipmentDto>> GetAllEquipmentByUserId(int userId);
        Task<bool> UpdateEquipment(int equipmentId, EquipmentDto equipmentDto);
    }
}