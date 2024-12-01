using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class EqiupmentService : IEqiupmentService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;

        public EqiupmentService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }

        // Pobieranie wszystkich elementów Equipment na podstawie roomId
        public async Task<List<EquipmentDto>> GetAllEquipmentByRoomId(int roomId)
        {
            try
            {
                var equipments = await _houseContext.Equipments
                    .Where(e => e.RoomId == roomId)
                    .ToListAsync()
                    ?? throw new Exception($"Watki dla pokoju o Id = {roomId} nie zostaly znalezione.");

                return _mapper.Map<List<EquipmentDto>>(equipments);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania urzadzen.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania urzadzen.", ex); }
        }

        // Pobieranie wszystkich elementów Equipment na podstawie UserId
        public async Task<List<EquipmentDto>> GetAllEquipmentByUserId(int userId)
        {
            try
            {
                var equipments = await _houseContext.Equipments
                    .Where(e => e.OwnerId == userId)
                    .ToListAsync()
                    ?? throw new Exception("Nie znaleziono urzadzen dla użytkownika o podanym iddentyfikatorze"); ;

                return _mapper.Map<List<EquipmentDto>>(equipments);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania urzadzen.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania urzadzen.", ex); }
        }

        // Tworzenie nowego elementu Equipment
        public async Task<int> CreateEquipment(EquipmentDto equipmentDto)
        {
            try
            {
                var equipmentEntity = _mapper.Map<Equipment>(equipmentDto);

                await _houseContext.Equipments.AddAsync(equipmentEntity);
                await _houseContext.SaveChangesAsync();

                return equipmentEntity.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas tworzenia urzadzenia.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas tworzenia urzadzenia.", ex); }
        }

        // Aktualizacja istniejącego elementu Equipment na podstawie Id
        public async Task<bool> UpdateEquipment(int equipmentId, EquipmentDto equipmentDto)
        {
            try
            {
                var existingEquipment = await _houseContext.Equipments
                    .FirstOrDefaultAsync(e => e.Id == equipmentId) ??
                    throw new Exception($"Urzadzenie o Id = {equipmentDto} nie zostalo znalezione.");

                if (existingEquipment == null) return false;

                // Mapowanie danych z DTO na istniejący obiekt
                _mapper.Map(equipmentDto, existingEquipment);
                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas aktualizacji urzadzenia.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas aktualizacji urzadzenia.", ex); }
        }

        // Usunięcie pojedynczego elementu Equipment na podstawie Id
        public async Task<bool> DeleteEquipment(int equipmentId)
        {
            try
            {
                var existingEquipment = await _houseContext.Equipments
                    .FirstOrDefaultAsync(e => e.Id == equipmentId);

                if (existingEquipment == null) return false;

                _houseContext.Equipments.Remove(existingEquipment);
                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas usuwania urzadzenia.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas usuwania urzadzenia.", ex); }
        }
    }
}
