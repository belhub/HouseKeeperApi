using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;

        public NotificationService(HouseKeeperDbContext houseKeeperContext, IMapper mapper)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
        }

        public async Task<NotificationDto> GetNotificationsForUserId(int userId)
        {
            try
            {
                var notyfications = await _houseContext.Notifications
                .FirstOrDefaultAsync(n => n.UserId == userId);

                return _mapper.Map<NotificationDto>(notyfications);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania powiadomien.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania powiadomien.", ex); }
        }

        public async Task<int> CreateNotification(NotificationDto notificationDto)
        {
            try
            {
                var notificationEntity = _mapper.Map<Notification>(notificationDto);

                await _houseContext.Notifications.AddAsync(notificationEntity);
                await _houseContext.SaveChangesAsync();

                return notificationEntity.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas tworzenia powiadomien.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas tworzenia powiadomien.", ex); }
        }

        public async Task<bool> UpdateNotification(NotificationDto notificationDto)
        {
            try
            {
                var notification = await _houseContext.Notifications
                    .FirstOrDefaultAsync(n => n.UserId == notificationDto.UserId) ??
                    throw new Exception($"user o id: {notificationDto.UserId} nie został znaleziony");

                if (notification == null) return false;

                _mapper.Map(notificationDto, notification);

                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas edycji powiadomien.", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas edycji powiadomien.", ex); }
        }
    }
}
