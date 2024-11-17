using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IRoomService
    {
        Task<int> CreateRoom(RoomDto roomDto);
        Task<bool> DeleteRoom(int roomId);
        Task<List<RoomDto>> GetRoomsByHouseId(int houseId);
        Task<List<RoomDto>> GetRoomsByOwnerId(int ownerId);
        Task<bool> UpdateRoom(int roomId, RoomDto roomDto);
    }
}