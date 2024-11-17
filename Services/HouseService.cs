using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class HouseService : IHouseService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;

        public HouseService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }

        public async Task<List<HouseDto>> GetHousesByUserId(int userId)
        {
            try
            {
                var houses = await _houseContext.Houses
                    .Where(h => h.OwnerId == userId)
                    .Include(h => h.Rooms)  // Załadowanie pokoi
                        .ThenInclude(r => r.Equipments) // Załadowanie sprzętu w pokojach
                        .AsNoTracking()  // Opcjonalnie
                    .ToListAsync() ?? throw new Exception("Nie znaleziono domu dla uzytkownika o podanym identyfikatorze");

                return _mapper.Map<List<HouseDto>>(houses);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania domu.", ex); }
        }

        public async Task<HouseDto> GetHouseById(int houseId)
        {
            try
            {
                var houses = await _houseContext.Houses
                    .Include(h => h.Rooms)  // Załadowanie pokoi
                        .ThenInclude(r => r.Equipments) // Załadowanie sprzętu w pokojach
                        .AsNoTracking()  // Opcjonalnie
                    .FirstOrDefaultAsync(h => h.Id == houseId) ?? throw new Exception("Nie znaleziono domu o podanym identyfikatorze");

                return _mapper.Map<HouseDto>(houses);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania domu.", ex); }
        }

        public async Task<int> CreateHouse(HouseDto houseDto)
        {
            try
            {
                var houseEntity = _mapper.Map<House>(houseDto);

                await _houseContext.Houses.AddAsync(houseEntity);
                await _houseContext.SaveChangesAsync();

                return houseEntity.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas tworzenia domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas tworzenia domu.", ex); }
        }

        public async Task<bool> UpdateHouse(int houseId, HouseDto houseDto)
        {
            try
            {
                var house = await _houseContext.Houses
                    .FirstOrDefaultAsync(h => h.Id == houseId) ?? throw new Exception($"Dom o Id = {houseId} nie zostal znaleziony.");

                _mapper.Map(houseDto, house);

                await _houseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas aktualizacji domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas aktualizacji domu.", ex); }
        }

        public async Task<bool> DeleteHouseById(int houseId)
        {
            try
            {
                var house = await _houseContext.Houses
                    .FirstOrDefaultAsync(h => h.Id == houseId);

                if (house == null) return false;

                _houseContext.Houses.Remove(house);
                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas usuwania domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas usuwania domu.", ex); }
        }
    }
}
