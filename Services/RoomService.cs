using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;

        public RoomService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }
        public async Task<List<RoomDto>> GetRoomsByHouseId(int houseId)
        {
            try
            {
                var rooms = await _houseContext.Rooms
                    .Where(r => r.HouseId == houseId)
                    .Include(r => r.Equipments)
                    .ToListAsync()
                    ?? throw new KeyNotFoundException($"Pokoje dla domu o Id = {houseId} nie zostal znaleziony.");

                return _mapper.Map<List<RoomDto>>(rooms);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania listy pokoi.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania listy pokoi.", ex); }
        }

        public async Task<List<RoomDto>> GetRoomsByOwnerId(int ownerId)
        {
            try
            {
                var rooms = await _houseContext.Rooms
                    .Include(r => r.House)
                    .Include(r => r.Equipments)
                    .Where(r => r.TenantId == ownerId || r.TenantId == null)
                    .ToListAsync()
                    ?? throw new KeyNotFoundException($"Pokoje dla usera o Id = {ownerId} nie zostal znaleziony.");

                return _mapper.Map<List<RoomDto>>(rooms);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania listy pokoi.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania listy pokoi.", ex); }
        }

        public async Task<int> CreateRoom(RoomDto roomDto)
        {
            try
            {
                var room = _mapper.Map<Room>(roomDto);

                await _houseContext.Rooms.AddAsync(room);
                await _houseContext.SaveChangesAsync();

                return room.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas tworzenia pokoju.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas  tworzenia pokoju.", ex); }
        }

        public async Task<bool> UpdateRoom(int roomId, RoomDto roomDto)
        {
            try
            {
                var existingRoom = await _houseContext.Rooms
                    .Include(r => r.Equipments)
                    .FirstOrDefaultAsync(r => r.Id == roomId)
                    ?? throw new KeyNotFoundException($"Pokoj o Id = {roomId} nie zostal znaleziony.");

                _mapper.Map(roomDto, existingRoom);

                await _houseContext.SaveChangesAsync();
                _mapper.Map<RoomDto>(existingRoom);

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas edycji pokoju.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas  edycji pokoju.", ex); }
        }

        public async Task<bool> DeleteRoom(int roomId)
        {
            try
            {
                var existingRoom = await _houseContext.Rooms
                    .FirstOrDefaultAsync(r => r.Id == roomId)
                    ?? throw new KeyNotFoundException($"Pokoj o Id = {roomId} nie zostal znaleziony.");

                _houseContext.Rooms.Remove(existingRoom);
                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas usuwania pokoju.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas  usuwania pokoju.", ex); }
        }
    }
}
