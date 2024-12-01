using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class ScheduleService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;

        public ScheduleService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }

        public async Task<List<Schedule>> GetScheduleByHouseId(int houseId)
        {
            try
            {
                var schedule = await _houseContext.Schedules
                    .Include(s => s.User)
                    .Where(s => s.HouseId == houseId)
                    .AsNoTracking()
                    .ToListAsync() ?? throw new Exception("nie znaleziono rekordow dla podanego domu");

                return schedule;
            }
            catch (DbUpdateException ex) { throw new Exception("Wystapil problem podczas pobierania grafiku", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad  podczas pobierania grafiku", ex); }
        }

        public async Task<int> CreateNewSchedule(ScheduleDto schedule)
        {
            try
            {
                var scheduleEntity = _mapper.Map<Schedule>(schedule);

                await _houseContext.Schedules.AddAsync(scheduleEntity);
                await _houseContext.SaveChangesAsync();

                return scheduleEntity.Id;
            }
            catch (DbUpdateException ex) { throw new Exception("Wystapil problem podczas tworzenia grafiku", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas tworzenia grafiku", ex); }
        }

        public async Task<bool> UpdateSchedule(ScheduleDto scheduleDto, int scheduleId)
        {
            try
            {
                var schedule = await _houseContext.Schedules
                    .FirstOrDefaultAsync(s => s.Id == scheduleId) ?? throw new Exception($"Grafik o Id = {scheduleId} nie zostal znaleziony.");

                _mapper.Map(scheduleDto, schedule);

                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new Exception("Wystapil problem podczas edycji grafiku", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas edycji grafiku", ex); }
        }
    }
}
