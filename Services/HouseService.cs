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
                    .ToListAsync() ?? throw new Exception("Nie znaleziono domu dla użytkownika o podanym iddentyfikatorze");
                var housesDto = _mapper.Map<List<HouseDto>>(houses);

                return housesDto;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas pobierania domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas pobierania domu.", ex); }
        }

        public async Task<HouseDto> GetHouseById(int houseId)
        {
            try
            {
                var houses = await _houseContext.Houses
                    .Include(h => h.Rooms)  // Załadowanie pokoi
                    .FirstOrDefaultAsync(h => h.Id == houseId) ?? throw new Exception("Nie znaleziono domu o podanym iddentyfikatorze");
                var houseDto = _mapper.Map<HouseDto>(houses);

                return houseDto;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas pobierania domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany błąd podczas pobierania domu.", ex); }
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
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas tworzenia domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany błąd podczas tworzenia domu.", ex); }
        }

        public async Task<bool> UpdateHouse(int houseId, HouseDto houseDto)
        {
            try
            {
                var house = await _houseContext.Houses
                    .FirstOrDefaultAsync(h => h.Id == houseId) ?? throw new Exception($"Dom o Id = {houseId} nie został znaleziony.");

                _mapper.Map(houseDto, house);

                await _houseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas aktualizacji domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas aktualizacji domu.", ex); }
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
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystąpił problem podczas usuwania domu.", ex); }
            catch (Exception ex) { throw new Exception("Wystąpił nieoczekiwany bład podczas usuwania domu.", ex); }

        }
    }
}
