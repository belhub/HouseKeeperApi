using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseKeeperApi.Services
{
    public class TrasnsactionService : ITrasnsactionService
    {
        private readonly HouseKeeperDbContext _houseContext;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public TrasnsactionService(HouseKeeperDbContext houseKeeperContext, IMapper mapper, INotificationService notificationService)
        {
            _houseContext = houseKeeperContext;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<List<TransactionDto>> GetAllTtransactionsFormUserId(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                //int currentPage = page ?? 1;
                //int currentPageSize = pageSize ?? 10;
                //if (currentPage < 1 || currentPageSize < 1)
                //{
                //    throw new ArgumentException("Rozmiar strony i numer strony nie moga byc mniejsze od 1");
                //}

                var transactions = await _houseContext.Transactions
                    .Where(t => (t.PayerId == userId || t.ReceiverId == userId) &&
                        t.TransacitonDate >= startDate &&
                        t.TransacitonDate <= endDate)
                    .Include(t => t.Receiver)
                    .Include(t => t.Payer)
                    .AsNoTracking()
                    .OrderByDescending(t => t.TransacitonDate)
                    //.Skip((currentPage - 1) * currentPageSize)
                    //.Take(currentPageSize)
                    .ToListAsync() ?? throw new Exception("Nie znaleziono transaakcji dla podanego uzytkownika");

                return _mapper.Map<List<TransactionDto>>(transactions);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania transakcji.", ex); }
            catch (Exception ex) { throw new InvalidOperationException("Wystapil nieoczekiwany blad podczas pobierania transakcji.", ex); }
        }

        public async Task<int> CreateTransaction(TransactionDto transactionDto, int userId)
        {
            try
            {
                var userNotificationId = transactionDto.PayerId == userId ? transactionDto.ReceiverId : transactionDto.PayerId;
                var iosNotificationExist = await _notificationService.GetNotificationsForUserId(userId);

                if (iosNotificationExist != null)
                {
                    var notificationDto = new NotificationDto
                    {
                        UserId = userNotificationId,
                        TransactionNotification = true,
                        IssueNotification = iosNotificationExist.IssueNotification
                    };
                    await _notificationService.UpdateNotification(notificationDto);
                }
                else
                {
                    var notificationDto = new NotificationDto
                    {
                        UserId = userNotificationId,
                        TransactionNotification = true,
                        IssueNotification = false
                    };
                    await _notificationService.CreateNotification(notificationDto);
                }

                var transactionEntity = _mapper.Map<Transaction>(transactionDto);

                await _houseContext.Transactions.AddAsync(transactionEntity);
                await _houseContext.SaveChangesAsync();

                return transactionEntity.Id;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas tworzenia transakcji.", ex); }
            catch (Exception ex) { throw new InvalidOperationException("Wystapil nieoczekiwany blad podczas tworzenia transakcji.", ex); }
        }

        public async Task<bool> UpdateTransaction(int transactionId, TransactionDto transactionDto)
        {
            try
            {
                var transactions = await _houseContext.Transactions
                    .FirstOrDefaultAsync(t => t.Id == transactionId)
                    ?? throw new Exception("Nie znaleziono transaakcji dla podanego uzytkownika");
                _mapper.Map(transactionDto, transactions);
                await _houseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas edycji transakcji.", ex); }
            catch (Exception ex) { throw new InvalidOperationException("Wystapil nieoczekiwany blad podczas edycji transakcji.", ex); }
        }

        public async Task<bool> DeleteTransaction(int transactionId)
        {
            try
            {
                var transaction = await _houseContext.Transactions
                    .FirstOrDefaultAsync(t => t.Id == transactionId);

                if (transaction == null) return false;

                _houseContext.Transactions.Remove(transaction);
                await _houseContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas usuwania transakcji.", ex); }
            catch (Exception ex) { throw new InvalidOperationException("Wystapil nieoczekiwany blad podczas usuwania transakcji.", ex); }
        }
    }
}
